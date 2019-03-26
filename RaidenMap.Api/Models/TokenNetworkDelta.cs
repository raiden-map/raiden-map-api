using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace RaidenMap.Api.Models
{
    public class TokenNetworkDelta : DeltaBase
    {
        [BsonElement("token")]
        public Token Token { get; set; }

        [BsonElement("tokenNetworkAddress")]
        public string TokenNetworkAddress { get; set; }

        [BsonElement("channelsCount")]
        public long ChannelsCount { get; set; }

        [BsonElement("openChannels")]
        public long OpenChannels { get; set; }

        [BsonElement("closedChannels")]
        public long ClosedChannels { get; set; }

        [BsonElement("settledChannels")]
        public long SettledChannels { get; set; }

        [BsonElement("avgChannelDeposit")]
        public long AvgChannelDeposit { get; set; }

        [BsonElement("totalDeposit")]
        public long TotalDeposit { get; set; }

        [BsonElement("users")]
        public long Users { get; set; }

        [BsonElement("blockNumber")]
        public long BlockNumber { get; set; }

        [BsonElement("modifiedChannels")]
        public List<Channel> ModifiedChannels { get; set; } = new List<Channel>();

        public override bool Equals(object obj)
        {
            return obj is TokenNetworkDelta aggregate &&
                   EqualityComparer<Token>.Default.Equals(Token, aggregate.Token) &&
                   TokenNetworkAddress == aggregate.TokenNetworkAddress &&
                   ChannelsCount == aggregate.ChannelsCount &&
                   OpenChannels == aggregate.OpenChannels &&
                   ClosedChannels == aggregate.ClosedChannels &&
                   SettledChannels == aggregate.SettledChannels &&
                   AvgChannelDeposit == aggregate.AvgChannelDeposit &&
                   TotalDeposit == aggregate.TotalDeposit &&
                   Users == aggregate.Users &&
                   BlockNumber == aggregate.BlockNumber;
        }

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(Token);
            hash.Add(TokenNetworkAddress);
            hash.Add(ChannelsCount);
            hash.Add(OpenChannels);
            hash.Add(ClosedChannels);
            hash.Add(SettledChannels);
            hash.Add(AvgChannelDeposit);
            hash.Add(TotalDeposit);
            hash.Add(Users);
            hash.Add(BlockNumber);
            return hash.ToHashCode();
        }
    }
}
