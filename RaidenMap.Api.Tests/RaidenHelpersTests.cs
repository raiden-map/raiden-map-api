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
        public void GetMergedTokenNetworkDeltas_WhenCalled_ReturnsCorrectlyMergedList(
            List<RaidenDelta> delta,
            RaidenSnapshot RaidenSnapshot,
            List<TokenNetworkDelta> expected)
        {
            var result = RaidenHelpers.GetMergedTokenNetworkDeltas(delta, RaidenSnapshot);

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

                var emptyDelta = new List<RaidenDelta>();
                var delta = NewRaidenDeltaList(tnAddress);

                var emptyState = new RaidenSnapshot();
                var state = new RaidenSnapshot { TokenNetworks = NewTnList(tnAddress) };

                var expected = NewTnList(tnAddress);
                var emptyExpected = new List<TokenNetworkDelta>();

                yield return new TestCaseData(emptyDelta, emptyState, emptyExpected);
                yield return new TestCaseData(delta, emptyState, expected);
                yield return new TestCaseData(emptyDelta, state, expected);
            }
        }

        private static TokenNetworkDelta NewTnAggregate(string tnAddress)
            => new TokenNetworkDelta { TokenNetworkAddress = tnAddress };

        private static List<TokenNetworkDelta> NewTnList(string tnAddress)
            => new List<TokenNetworkDelta> { NewTnAggregate(tnAddress) };

        private static List<RaidenDelta> NewRaidenDeltaList(string tnAddress)
            => new List<RaidenDelta> { new RaidenDelta { TokenNetworkChanges = NewTnList(tnAddress) } };
    }

}