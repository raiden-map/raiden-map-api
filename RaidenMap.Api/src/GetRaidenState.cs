using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Logging;
using System.Linq;
using MongoDB.Driver;
using RaidenMap.Api.models;

namespace RaidenMap.Api
{
    public static class GetRaidenState
    {
        private const string DatabaseName = "raiden-map";
        private const string CollectionName = "raiden-states";

        private static string MongoDbConnectionString =>
            System.Environment.GetEnvironmentVariable("MongoDbConnectionString");

        [FunctionName("GetRaidenState")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "raiden")] HttpRequest req,
            ILogger log)
        {
            var client = new MongoClient(MongoDbConnectionString);

            var raidenStates = 
                client
                    .GetDatabase(DatabaseName)
                    .GetCollection<Raiden>(CollectionName);

            var filter = new FilterDefinitionBuilder<Raiden>();

            var stateCursor = await raidenStates.FindAsync(filter.Empty);

            var state = stateCursor.ToEnumerable().OrderByDescending(r => r.BlockNumber).FirstOrDefault();

            return new OkObjectResult(state);
        }

    }
}
