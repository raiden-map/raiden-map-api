using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Collections.Generic;
using System;

namespace RaidenMap.Api.Models
{
    public class TokenNetworkSnapshot
    {
        [BsonId]
        public ObjectId MongoId { get; set; }

        [BsonElement("tokenNetworkAddress")]
        public string TokenNetworkAddress { get; set; }

        [BsonElement("tokenNetworkStates")]
        public List<TokenNetworkDelta> TokenNetworkDeltas { get; set; } = new List<TokenNetworkDelta>();

        [BsonElement("creationBlockNumber")]
        public long CreationBlockNumber { get; set; }

        [BsonElement("creationTimestamp")]
        public long CreationTimestamp { get; set; }

        [BsonElement("creationBlockNumber")]
        public long StateBlockNumber { get; set; }

        [BsonElement("creationTimestamp")]
        public long StateTimestamp { get; set; }

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

        public override bool Equals(object obj)
        {
            return obj is TokenNetworkSnapshot state &&
                   MongoId.Equals(state.MongoId) &&
                   TokenNetworkAddress == state.TokenNetworkAddress &&
                   EqualityComparer<List<TokenNetworkDelta>>.Default.Equals(TokenNetworkDeltas, state.TokenNetworkDeltas) &&
                   CreationBlockNumber == state.CreationBlockNumber &&
                   CreationTimestamp == state.CreationTimestamp &&
                   StateBlockNumber == state.StateBlockNumber &&
                   StateTimestamp == state.StateTimestamp &&
                   EqualityComparer<Token>.Default.Equals(Token, state.Token) &&
                   EqualityComparer<List<Channel>>.Default.Equals(Channels, state.Channels) &&
                   EqualityComparer<List<Endpoint>>.Default.Equals(Endpoints, state.Endpoints) &&
                   Twitter == state.Twitter &&
                   Id == state.Id;
        }

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(MongoId);
            hash.Add(TokenNetworkAddress);
            hash.Add(TokenNetworkDeltas);
            hash.Add(CreationBlockNumber);
            hash.Add(CreationTimestamp);
            hash.Add(StateBlockNumber);
            hash.Add(StateTimestamp);
            hash.Add(Token);
            hash.Add(Channels);
            hash.Add(Endpoints);
            hash.Add(Twitter);
            hash.Add(Id);
            return hash.ToHashCode();
        }
    }

}
