using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
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

        [BsonElement("twitter")]
        public Uri Twitter { get; set; }

        [BsonElement("states")]
        public RaidenAggregate[] States { get; set; }

        [BsonElement("tokenNetworks")]
        public TokenNetworkAggregate[] TokenNetworks { get; set; }

        [BsonElement("endpoints")]
        public Endpoint[] Endpoints { get; set; }

        [BsonElement("id")]
        public Guid Id { get; set; }

        public static Raiden FromJson(string json) => JsonConvert.DeserializeObject<Raiden>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this Raiden self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
