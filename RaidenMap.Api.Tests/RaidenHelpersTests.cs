using NUnit.Framework;
using RaidenMap.Api.Models;
using RaidenMap.Api.Utility;
using System.Collections.Generic;

namespace RaidenMap.Api.Tests
{
    [TestFixture]
    public class RaidenHelpersTests
    {
        [Test, TestCaseSource(typeof(RaidenHelpersTestCases), nameof(RaidenHelpersTestCases.OneOrBothEmpty))]
        public void GetTokenNetworkChanges_WhenCalled_ReturnsCorrectlyMergedList(
            List<RaidenAggregate> delta,
            RaidenState raidenState,
            List<TokenNetworkAggregate> expected)
        {
            var result = RaidenHelpers.GetMergedTokenNetworkAggregates(delta, raidenState);

            Assert.That(result, Is.EquivalentTo(expected));
        }

    }

    public class RaidenHelpersTestCases
    {
        public static IEnumerable<TestCaseData> OneOrBothEmpty
        {
            get
            {
                var tnAddress = "0x33";

                var emptyDelta = new List<RaidenAggregate>();
                var delta = NewRaidenAggregateList(tnAddress);

                var emptyState = new RaidenState();
                var state = new RaidenState { TokenNetworks = NewTnList(tnAddress) };

                var expected = NewTnList(tnAddress);
                var emptyExpected = new List<TokenNetworkAggregate>();

                yield return new TestCaseData(emptyDelta, emptyState, emptyExpected);
                yield return new TestCaseData(delta, emptyState, expected);
                yield return new TestCaseData(emptyDelta, state, expected);
            }
        }

        private static TokenNetworkAggregate NewTnAggregate(string tnAddress)
            => new TokenNetworkAggregate { TokenNetworkAddress = tnAddress };

        private static List<TokenNetworkAggregate> NewTnList(string tnAddress)
            => new List<TokenNetworkAggregate> { NewTnAggregate(tnAddress) };

        private static List<RaidenAggregate> NewRaidenAggregateList(string tnAddress)
            => new List<RaidenAggregate> { new RaidenAggregate { TokenNetworkChanges = NewTnList(tnAddress) } };
    }

}