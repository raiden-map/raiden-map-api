using MongoDB.Bson.Serialization.Attributes;

namespace RaidenMap.Api.Models
{
    public class TokenNetworkAggregate
    {
        [BsonElement("token")]
        public Token Token { get; set; }

        [BsonElement("tokenNetworkAddress")]
        public string TokenNetworkAddress { get; set; }

        [BsonElement("timestamp")]
        public long Timestamp { get; set; }

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

        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(TokenNetworkAggregate))
                return false;

            var tn = obj as TokenNetworkAggregate;

            return this.TokenNetworkAddress == tn.TokenNetworkAddress;
        }
    }
}
