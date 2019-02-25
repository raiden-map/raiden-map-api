using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using RaidenMap.Api.Models;
using RaidenMap.Api.src.Common;
using RaidenMap.Api.src.Utility;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using System;

namespace RaidenMap.Api
{
    public static class GetPastRaidenState
    {
        private static string DatabaseName =>
            System.Environment.GetEnvironmentVariable(Constants.DatabaseName);

        private static string RaidenCollection =>
            System.Environment.GetEnvironmentVariable(Constants.RaidenCollectionName);

        private static string RaidenAggregateCollection =>
            System.Environment.GetEnvironmentVariable(Constants.RaidenAggregateCollectionName);

        private static string MongoDbConnectionString =>
            System.Environment.GetEnvironmentVariable("MongoDbConnectionString");

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
                await RetrieveDelta(
                    nearestRaidenState.Timestamp,
                    timestamp,
                    client
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

        private static async Task<(Raiden, IMongoCollection<Raiden>)> RetrieveNearestState(long timestamp, MongoClient client)
        {
            var raidenStates =
                client
                    .GetDatabase(DatabaseName)
                    .GetCollection<Raiden>(RaidenCollection);

            var filter = new FilterDefinitionBuilder<Raiden>()
                .Where(rs => rs.Timestamp <= timestamp);

            var stateCursor = await raidenStates.FindAsync(filter);

            var state =
                stateCursor
                    .ToEnumerable()
                    .OrderByDescending(r => r.Timestamp)
                    .FirstOrDefault();

            return (state, raidenStates);
        }

        private static async Task<(List<RaidenAggregate>, IMongoCollection<RaidenAggregate>)> RetrieveDelta(
            long fromTimestamp, long toTimestamp, MongoClient client)
        {
            var raidenAggregates =
                client
                    .GetDatabase(DatabaseName)
                    .GetCollection<RaidenAggregate>(RaidenAggregateCollection);

            var filter = new FilterDefinitionBuilder<RaidenAggregate>()
                .Where(agg => agg.Timestamp >= fromTimestamp && agg.Timestamp <= toTimestamp);

            var stateCursor = await raidenAggregates.FindAsync(filter);

            var aggregates = await stateCursor.ToListAsync();

            return (aggregates, raidenAggregates);
        }
    }
}
