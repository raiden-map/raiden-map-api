using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RaidenMap.Api.Models
{
    public partial class RaidenSnapshot
    {
        [BsonId]
        public ObjectId MongoId { get; set; }

        [BsonElement("blockNumber")]
        public long BlockNumber { get; set; }

        [BsonElement("timestamp")] 
        public long Timestamp { get; set; }

        [BsonElement("twitter")]
        public Uri Twitter { get; set; }

        [BsonElement("states")]
        public List<RaidenDelta> States { get; set; } = new List<RaidenDelta>();

        [BsonElement("tokenNetworks")]
        public List<TokenNetworkDelta> TokenNetworks { get; set; } = new List<TokenNetworkDelta>();

        [BsonElement("endpoints")]
        public List<Endpoint> Endpoints { get; set; } = new List<Endpoint>();

        [BsonElement("id")]
        public string Id { get; set; }

        public override bool Equals(object obj)
        {
            return obj is RaidenSnapshot state &&
                   MongoId.Equals(state.MongoId) &&
                   BlockNumber == state.BlockNumber &&
                   Timestamp == state.Timestamp &&
                   EqualityComparer<Uri>.Default.Equals(Twitter, state.Twitter) &&
                   EqualityComparer<List<RaidenDelta>>.Default.Equals(States, state.States) &&
                   EqualityComparer<List<TokenNetworkDelta>>.Default.Equals(TokenNetworks, state.TokenNetworks) &&
                   EqualityComparer<List<Endpoint>>.Default.Equals(Endpoints, state.Endpoints) &&
                   Id == state.Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(MongoId, BlockNumber, Timestamp, Twitter, States, TokenNetworks, Endpoints, Id);
        }
    }

}
