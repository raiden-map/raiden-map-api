using RaidenMap.Api.Common;
using RaidenMap.Api.Models;
using System.Collections.Generic;
using System.Linq;

namespace RaidenMap.Api.Utility
{
    public static class RaidenHelpers
    {
        public static List<TokenNetworkAggregate> GetMergedTokenNetworkAggregates(List<RaidenAggregate> delta, RaidenState raidenState)
        {
            var tokenNetworksState =
                delta.Aggregate(
                    raidenState.TokenNetworks,
                    (list, raidenAggregate) =>
                    {
                        var oldValues =
                            list
                                .Join(
                                    raidenAggregate.TokenNetworkChanges,
                                    inner => inner.TokenNetworkAddress,
                                    outer => outer.TokenNetworkAddress,
                                    (inner, outer) => outer ?? inner ?? new TokenNetworkAggregate()
                                );


                        foreach (var oldValue in oldValues)
                            list.Remove(oldValue);

                        list.AddRange(raidenAggregate.TokenNetworkChanges);

                        return list;
                    }
                );

            return tokenNetworksState ?? new List<TokenNetworkAggregate>();
        }

        public static bool TimestampsAreClose(long t1, long t2) =>
            t2 >= t1 - Constants.TimeStampDelta && t2 <= t1 + Constants.TimeStampDelta;
    }
}
