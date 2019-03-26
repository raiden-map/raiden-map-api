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
        public static List<TokenNetworkDelta> GetMergedTokenNetworkDeltas(List<RaidenDelta> delta, RaidenSnapshot raidenState)
        {
            var tokenNetworksStates =
                delta.Aggregate(
                    raidenState.TokenNetworks,
                    (list, raidenAggregate) => JoinTnAggregates(list, raidenAggregate.TokenNetworkChanges)
                );

            return tokenNetworksStates ?? new List<TokenNetworkDelta>();
        }

        public static List<TokenNetworkDelta> GetMergedTokenNetworkDeltas(List<TokenNetworkDelta> delta, TokenNetworkSnapshot tnState)
        {
            var tokenNetworksStates = JoinTnAggregates(tnState.TokenNetworkDeltas, delta);

            return tokenNetworksStates ?? new List<TokenNetworkDelta>();
        }

        public static List<Channel> GetMergedChannels(List<TokenNetworkDelta> deltas, TokenNetworkSnapshot snap)
        {
            var modifiedChannels =
                deltas
                    .Aggregate(
                        snap.Channels,
                        (channels, delta) => MergeChannels(channels, delta.ModifiedChannels)
                    );

            return modifiedChannels ?? new List<Channel>();
        }

        private static List<Channel> MergeChannels(List<Channel> oldChannels, List<Channel> newChannels)
        {
            oldChannels
                .RemoveAll(
                    ch => newChannels.Any(x => x.ChannelId == ch.ChannelId)
                );

            oldChannels.AddRange(newChannels);

            return oldChannels;
        }

        private static List<TokenNetworkDelta> JoinTnAggregates(List<TokenNetworkDelta> aggs, List<TokenNetworkDelta> newAggs)
        {
            var oldValues =
                aggs
                    .Join(
                        newAggs,
                        inner => inner.TokenNetworkAddress,
                        outer => outer.TokenNetworkAddress,
                        (inner, outer) => outer ?? inner ?? new TokenNetworkDelta()
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
        ) where T : DeltaBase
        {
            var aggregatesCollection =
                client
                    .GetDatabase(dbName)
                    .GetCollection<T>(aggCollectionName);

            var filter = new FilterDefinitionBuilder<T>()
                .Where(agg => agg.Timestamp >= fromTimestamp && agg.Timestamp <= toTimestamp);

            var aggCursor = await aggregatesCollection.FindAsync(filter);

            var aggregates = await aggCursor.ToListAsync();

            return (aggregates, aggregatesCollection);
        }

        public static bool TimestampsAreClose(long t1, long t2) =>
            t2 >= t1 - Constants.TimeStampDelta && t2 <= t1 + Constants.TimeStampDelta;
    }


}
