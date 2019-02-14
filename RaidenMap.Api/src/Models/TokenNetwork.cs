using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Collections.Generic;

namespace RaidenMap.Api.Models
{
    public class TokenNetwork
    {
        [BsonId]
        public ObjectId MongoId { get; set; }

        [BsonElement("tokenNetworkAddress")]
        public string TokenNetworkAddress { get; set; }

        [BsonElement("tokenNetworkStates")]
        public List<TokenNetworkAggregate> TokenNetworkStates { get; set; } = new List<TokenNetworkAggregate>();

        [BsonElement("creationBlockNumber")]
        public long CreationBlockNumber { get; set; }

        [BsonElement("creationTimestamp")]
        public long CreationTimestamp { get; set; }

        [BsonElement("token")]
        public Token Token { get; set; }

        [BsonElement("channels")]
        public List<Channel> Channels { get; set; } = new List<Channel>();

        [BsonElement("endpoints")]
        public List<Endpoint> Endpoints { get; set; } = new List<Endpoint>();

        [BsonElement("twitter")]
        public string Twitter { get; set; }

        [BsonElement("id")]
        public string Id { get; set; }

    }

}
