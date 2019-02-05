using MongoDB.Bson.Serialization.Attributes;

namespace RaidenMap.Api.Models
{
    public class Token
    {
        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("tag")]
        public string Tag { get; set; }

        [BsonElement("imageUrl")]
        public string ImageUrl { get; set; }

        [BsonElement("value")]
        public long Value { get; set; }

        [BsonElement("changeSinceYesterday")]
        public double ChangeSinceYesterday { get; set; }

        [BsonElement("marketCap")]
        public double MarketCap { get; set; }

        [BsonElement("volume")]
        public long Volume { get; set; }

        [BsonElement("holders")]
        public long Holders { get; set; }

        [BsonElement("timestamp")]
        public long Timestamp { get; set; }
    }
}
