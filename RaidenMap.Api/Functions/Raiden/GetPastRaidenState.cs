using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using RaidenMap.Api.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using System;
using RaidenMap.Api.Utility;
using RaidenMap.Api.Common;

namespace RaidenMap.Api.Functions.Raiden
{
    public static class GetPastRaidenState
    {
        private static string DatabaseName =>
            Environment.GetEnvironmentVariable(Constants.DatabaseName);

        private static string RaidenCollection =>
            Environment.GetEnvironmentVariable(Constants.RaidenCollectionName);

        private static string RaidenAggregateCollection =>
            Environment.GetEnvironmentVariable(Constants.RaidenAggregateCollectionName);

        private static string MongoDbConnectionString =>
            Environment.GetEnvironmentVariable("MongoDbConnectionString");

        [FunctionName("GetPastRaidenState")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v1/raiden/{timestamp}")] HttpRequest req,
            long timestamp,
            ILogger log)
        {

            var client = new MongoClient(MongoDbConnectionString);

            log.LogInformation($"{DateTime.UtcNow} INFO Retrievieng Nearest State.");
            var (nearestRaidenState, states) = await RetrieveNearestState(timestamp, client);

            if (RaidenHelpers.TimestampsAreClose(nearestRaidenState.Timestamp, timestamp))
            {
                log.LogInformation($"{DateTime.UtcNow} INFO A near enough state was already available.");
                return new OkObjectResult(nearestRaidenState);
            }

            log.LogInformation($"{DateTime.UtcNow} INFO Computing Delta.");
            var (delta, aggregates) =
                await RaidenHelpers.RetrieveDelta<RaidenAggregate>(
                    nearestRaidenState.Timestamp,
                    timestamp,
                    client,
                    DatabaseName,
                    RaidenAggregateCollection
                );

            var raidenStates =
                nearestRaidenState
                    .States
                    .OrderByDescending(s => s.BlockNumber)
                    .ToList();

            var howManyToRemove = delta.Count() - 1;
            var howManyToRetain = nearestRaidenState.States.Count() - delta.Count();

            // Remove unneded aggregates
            raidenStates.RemoveRange(howManyToRetain, howManyToRemove);
            raidenStates.AddRange(delta);

            nearestRaidenState.BlockNumber = delta.Max(x => x.BlockNumber);
            nearestRaidenState.Timestamp = delta.Max(x => x.Timestamp);

            nearestRaidenState.TokenNetworks = RaidenHelpers.GetMergedTokenNetworkAggregates(delta, nearestRaidenState);

            nearestRaidenState.States = raidenStates;

            nearestRaidenState.MongoId = new ObjectId();

            log.LogInformation($"{DateTime.UtcNow} INFO Inserting newly computed state in database");
            // Could run in a separate Task
            await states.InsertOneAsync(nearestRaidenState);

            log.LogInformation($"{DateTime.UtcNow} INFO Returning computed state.");
            return new OkObjectResult(nearestRaidenState);
        }

        private static async Task<(RaidenState, IMongoCollection<RaidenState>)> RetrieveNearestState(long timestamp, MongoClient client)
        {
            var raidenStates =
                client
                    .GetDatabase(DatabaseName)
                    .GetCollection<RaidenState>(RaidenCollection);

            var filter = new FilterDefinitionBuilder<RaidenState>()
                .Where(rs => rs.Timestamp <= timestamp);

            var stateCursor = await raidenStates.FindAsync(filter);

            var state =
                stateCursor
                    .ToEnumerable()
                    .OrderByDescending(r => r.Timestamp)
                    .FirstOrDefault();

            return (state, raidenStates);
        }
    }
}
