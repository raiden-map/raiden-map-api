using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RaidenMap.Api.Models
{
    public class RaidenAggregate
    {
        [BsonId]
        public ObjectId MongoId { get; set; }

        [BsonElement("tokenNetworksCount")]
        public long TokenNetworksCount { get; set; }

        [BsonElement("usersCount")]
        public long UsersCount { get; set; }

        [BsonElement("timestamp")]
        public long Timestamp { get; set; }

        [BsonElement("blockNumber")]
        public long BlockNumber { get; set; }

        [BsonElement("btcValue")]
        public long BtcValue { get; set; }

        [BsonElement("ethValue")]
        public long EthValue { get; set; }

        [BsonElement("id")]
        public string Id { get; set; }

        [BsonElement("tokenNetworksChanges")]
        public List<TokenNetworkAggregate> TokenNetworkChanges { get; set; }
    }
}
