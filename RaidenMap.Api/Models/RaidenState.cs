using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RaidenMap.Api.Models
{
    public partial class RaidenState
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
        public List<RaidenAggregate> States { get; set; } = new List<RaidenAggregate>();

        [BsonElement("tokenNetworks")]
        public List<TokenNetworkAggregate> TokenNetworks { get; set; } = new List<TokenNetworkAggregate>();

        [BsonElement("endpoints")]
        public List<Endpoint> Endpoints { get; set; } = new List<Endpoint>();

        [BsonElement("id")]
        public string Id { get; set; }

        public override bool Equals(object obj)
        {
            return obj is RaidenState state &&
                   MongoId.Equals(state.MongoId) &&
                   BlockNumber == state.BlockNumber &&
                   Timestamp == state.Timestamp &&
                   EqualityComparer<Uri>.Default.Equals(Twitter, state.Twitter) &&
                   EqualityComparer<List<RaidenAggregate>>.Default.Equals(States, state.States) &&
                   EqualityComparer<List<TokenNetworkAggregate>>.Default.Equals(TokenNetworks, state.TokenNetworks) &&
                   EqualityComparer<List<Endpoint>>.Default.Equals(Endpoints, state.Endpoints) &&
                   Id == state.Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(MongoId, BlockNumber, Timestamp, Twitter, States, TokenNetworks, Endpoints, Id);
        }
    }

}
