using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace RaidenMap.Api.Raiden
{
    public static class GetRaidenState
    {
        private const string DatabaseName = "raiden-map";
        private const string CollectionName = "raiden-states";

        private static string MongoDbConnectionString =>
            System.Environment.GetEnvironmentVariable("MongoDbConnectionString");

        [FunctionName("GetRaidenState")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v1/raiden")] HttpRequest req,
            ILogger log)
        {
            var client = new MongoClient(MongoDbConnectionString);

            var raidenStates =
                client
                    .GetDatabase(DatabaseName)
                    .GetCollection<Models.Raiden>(CollectionName);

            var filter = new FilterDefinitionBuilder<Models.Raiden>();

            var stateCursor = await raidenStates.FindAsync(filter.Empty);

            var state = stateCursor.ToEnumerable().OrderByDescending(r => r.BlockNumber).FirstOrDefault();

            return new OkObjectResult(state);
        }

    }
}
