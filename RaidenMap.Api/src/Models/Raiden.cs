using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RaidenMap.Api.Models
{
    public partial class Raiden
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
        public List<RaidenAggregate> States { get; set; }

        [BsonElement("tokenNetworks")]
        public List<TokenNetworkAggregate> TokenNetworks { get; set; }

        [BsonElement("endpoints")]
        public List<Endpoint> Endpoints { get; set; }

        [BsonElement("id")]
        public string Id { get; set; }
    }

}
