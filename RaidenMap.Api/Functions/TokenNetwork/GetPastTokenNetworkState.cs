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

namespace RaidenMap.Api.Functions.TokenNetwork
{
    public static class GetPastTokenNetworkState
    {
        private static string DatabaseName =>
            Environment.GetEnvironmentVariable(Constants.DatabaseName);

        private static string TokenNetworkCollection =>
            Environment.GetEnvironmentVariable(Constants.TokenNetworkCollectionName);

        private static string TokenNetworkAggregateCollection =>
            Environment.GetEnvironmentVariable(Constants.TokenNetworkAggregateCollectionName);

        private static string MongoDbConnectionString =>
            Environment.GetEnvironmentVariable(Constants.MongoDbConnectionString);

        [FunctionName("GetPastTokenNetworkState")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v1/tokenNetwork/{tnAddress}/{timestamp}")] HttpRequest req,
            string tnAddress,
            long timestamp,
            ILogger log
        ){
            var client = new MongoClient(MongoDbConnectionString);

            log.LogInformation($"{DateTime.UtcNow} INFO Retrievieng Nearest State.");
            var (nearestTnState, states) = await RetrieveNearestState(timestamp, tnAddress, client);

            if (RaidenHelpers.TimestampsAreClose(nearestTnState.StateTimestamp, timestamp))
            {
                log.LogInformation($"{DateTime.UtcNow} INFO A near enough state was already available.");
                return new OkObjectResult(nearestTnState);
            }

            log.LogInformation($"{DateTime.UtcNow} INFO Computing Delta.");
            var (delta, aggregates) =
                await RaidenHelpers.RetrieveDelta<TokenNetworkAggregate>(
                    nearestTnState.StateTimestamp,
                    timestamp,
                    client,
                    DatabaseName,
                    TokenNetworkAggregateCollection
                );

            var tnStates =
                nearestTnState
                    .TokenNetworkStates
                    .OrderByDescending(s => s.BlockNumber)
                    .ToList();

            var howManyToRemove = delta.Count() - 1;
            var howManyToRetain = nearestTnState.TokenNetworkStates.Count() - delta.Count();

            // Remove unneded aggregates
            tnStates.RemoveRange(howManyToRetain, howManyToRemove);
            tnStates.AddRange(delta);

            nearestTnState.StateBlockNumber = delta.Max(x => x.BlockNumber);
            nearestTnState.StateTimestamp = delta.Max(x => x.Timestamp);

            nearestTnState.TokenNetworkStates = RaidenHelpers.GetMergedTokenNetworkAggregates(delta, nearestTnState);

            nearestTnState.TokenNetworkStates = tnStates;

            nearestTnState.MongoId = new ObjectId();

            log.LogInformation($"{DateTime.UtcNow} INFO Inserting newly computed state in database");
            // Could run in a separate Task
            await states.InsertOneAsync(nearestTnState);

            log.LogInformation($"{DateTime.UtcNow} INFO Returning computed state.");
            return new OkObjectResult(nearestTnState);
        }

        private static async Task<(TokenNetworkState, IMongoCollection<TokenNetworkState>)> RetrieveNearestState(
            long timestamp, 
            string tnAddress, 
            MongoClient client
        ){
            var tnStates =
                client
                    .GetDatabase(DatabaseName)
                    .GetCollection<TokenNetworkState>(TokenNetworkCollection);

            var filter = new FilterDefinitionBuilder<TokenNetworkState>()
                .Where(tns => tns.StateTimestamp <= timestamp && tns.TokenNetworkAddress == tnAddress);

            var stateCursor = await tnStates.FindAsync(filter);

            var state =
                stateCursor
                    .ToEnumerable()
                    .OrderByDescending(tn => tn.StateTimestamp)
                    .FirstOrDefault();

            return (state, tnStates);
        }
    }
}
