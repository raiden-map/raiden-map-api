using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RaidenMap.Api.Models
{
    public class RaidenDelta : DeltaBase
    {
        [BsonId]
        public ObjectId MongoId { get; set; }

        [BsonElement("tokenNetworksCount")]
        public long TokenNetworksCount { get; set; }

        [BsonElement("usersCount")]
        public long UsersCount { get; set; }

        [BsonElement("blockNumber")]
        public long BlockNumber { get; set; }

        [BsonElement("btcValue")]
        public long BtcValue { get; set; }

        [BsonElement("ethValue")]
        public long EthValue { get; set; }

        [BsonElement("id")]
        public string Id { get; set; }

        [BsonElement("tokenNetworksChanges")]
        public List<TokenNetworkDelta> TokenNetworkChanges { get; set; } = new List<TokenNetworkDelta>();

        public override bool Equals(object obj)
        {
            return obj is RaidenDelta aggregate &&
                   MongoId.Equals(aggregate.MongoId) &&
                   TokenNetworksCount == aggregate.TokenNetworksCount &&
                   UsersCount == aggregate.UsersCount &&
                   BlockNumber == aggregate.BlockNumber &&
                   BtcValue == aggregate.BtcValue &&
                   EthValue == aggregate.EthValue &&
                   Id == aggregate.Id &&
                   EqualityComparer<List<TokenNetworkDelta>>.Default.Equals(TokenNetworkChanges, aggregate.TokenNetworkChanges);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(MongoId, TokenNetworksCount, UsersCount, BlockNumber, BtcValue, EthValue, Id, TokenNetworkChanges);
        }
    }
}
