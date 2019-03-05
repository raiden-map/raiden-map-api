using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using RaidenMap.Api.Models;
using System.Linq;
using System.Threading.Tasks;
using RaidenMap.Api.Common;

namespace RaidenMap.Api.Functions.TokenNetwork
{
    public static class GetTokenNetworkState
    {
        private static string DatabaseName =>
            System.Environment.GetEnvironmentVariable(Constants.DatabaseName);

        private static string CollectionName =>
            System.Environment.GetEnvironmentVariable(Constants.TokenNetworkCollectionName);

        private static string MongoDbConnectionString =>
            System.Environment.GetEnvironmentVariable(Constants.MongoDbConnectionString);

        [FunctionName("GetTokenNetworkState")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v1/tokenNetwork/{tnAddress}")] HttpRequest req,
            string tnAddress,
            ILogger log)
        {
            var client = new MongoClient(MongoDbConnectionString);

            var tokenNetworks =
                client
                    .GetDatabase(DatabaseName)
                    .GetCollection<TokenNetworkState>(CollectionName);

            var filter = new FilterDefinitionBuilder<TokenNetworkState>()
                    .Where(tn => tn.TokenNetworkAddress == tnAddress);

            var stateCursor = await tokenNetworks.FindAsync(filter);

            var state =
                stateCursor
                    .ToEnumerable()
                    .FirstOrDefault();

            return state is null
                ? (IActionResult)new NotFoundResult()
                : (IActionResult)new OkObjectResult(state);
        }

    }
}