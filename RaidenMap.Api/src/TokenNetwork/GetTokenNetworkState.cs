using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System.Threading.Tasks;
using RaidenMap.Api.Models;

namespace RaidenMap.Api
{
    public static class GetTokenNetworkState
    {
        private const string DatabaseName = "raiden-map";
        private const string CollectionName = "token-network-states";

        private static string MongoDbConnectionString =>
            System.Environment.GetEnvironmentVariable("MongoDbConnectionString");

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
                    .GetCollection<TokenNetwork>(CollectionName);

            var filter = new FilterDefinitionBuilder<TokenNetwork>()
                .Where(tn => tn.TokenNetworkAddress == tnAddress);

            var stateCursor = await tokenNetworks.FindAsync(filter);

            var state = stateCursor.Current?.FirstOrDefault();

            return state is null 
                ? (IActionResult) new NotFoundResult() 
                : (IActionResult) new OkObjectResult(state);
        }

    }
}
