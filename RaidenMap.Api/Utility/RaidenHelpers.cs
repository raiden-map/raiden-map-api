using MongoDB.Driver;
using RaidenMap.Api.Common;
using RaidenMap.Api.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RaidenMap.Api.Utility
{
    public static class RaidenHelpers
    {
        public static List<TokenNetworkAggregate> GetMergedTokenNetworkAggregates(List<RaidenAggregate> delta, RaidenState raidenState)
        {
            var tokenNetworksStates =
                delta.Aggregate(
                    raidenState.TokenNetworks,
                    (list, raidenAggregate) => JoinTnAggregates(list, raidenAggregate.TokenNetworkChanges)
                );

            return tokenNetworksStates ?? new List<TokenNetworkAggregate>();
        }

        public static List<TokenNetworkAggregate> GetMergedTokenNetworkAggregates(List<TokenNetworkAggregate> delta, TokenNetworkState tnState)
        {
            var tokenNetworksStates = JoinTnAggregates(tnState.TokenNetworkStates, delta);

            return tokenNetworksStates ?? new List<TokenNetworkAggregate>();
        }

        private static List<TokenNetworkAggregate> JoinTnAggregates(List<TokenNetworkAggregate> aggs, List<TokenNetworkAggregate> newAggs)
        {
            var oldValues =
                aggs
                    .Join(
                        newAggs,
                        inner => inner.TokenNetworkAddress,
                        outer => outer.TokenNetworkAddress,
                        (inner, outer) => outer ?? inner ?? new TokenNetworkAggregate()
                    );


            foreach (var oldValue in oldValues)
                aggs.Remove(oldValue);

            aggs.AddRange(newAggs);

            return aggs;
        }

        public static async Task<(List<T>, IMongoCollection<T>)> RetrieveDelta<T>(
            long fromTimestamp, 
            long toTimestamp, 
            MongoClient client, 
            string dbName,
            string aggCollectionName
        ) where T : AggregateBase
        {
            var tnAggregates =
                client
                    .GetDatabase(dbName)
                    .GetCollection<T>(aggCollectionName);

            var filter = new FilterDefinitionBuilder<T>()
                .Where(agg => agg.Timestamp >= fromTimestamp && agg.Timestamp <= toTimestamp);

            var aggCursor = await tnAggregates.FindAsync(filter);

            var aggregates = await aggCursor.ToListAsync();

            return (aggregates, tnAggregates);
        }

        public static bool TimestampsAreClose(long t1, long t2) =>
            t2 >= t1 - Constants.TimeStampDelta && t2 <= t1 + Constants.TimeStampDelta;
    }


}
