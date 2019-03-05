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

        [BsonElement("valueUsd")]
        public long ValueUsd { get; set; }

        [BsonElement("value")]
        public long ValueEth { get; set; }

        [BsonElement("valueBtc")]
        public long ValueBtc { get; set; }

        [BsonElement("priceChangeDayUsd")]
        public double PriceChangeDayUsd { get; set; }

        [BsonElement("priceChangeWeekUsd")]
        public double PriceChangeWeekUsd { get; set; }

        [BsonElement("priceChangeDayEth")]
        public double PriceChangeDayEth { get; set; }

        [BsonElement("priceChangeWeekEth")]
        public double PriceChangeWeekEth { get; set; }

        [BsonElement("priceChangeDayBtc")]
        public double PriceChangeDayBtc { get; set; }

        [BsonElement("priceChangeWeekBtc")]
        public double PriceChangeWeekBtc { get; set; }

        [BsonElement("marketCap")]
        public double MarketCap { get; set; }

        [BsonElement("volume")]
        public long Volume { get; set; }

        [BsonElement("timestamp")]
        public long Timestamp { get; set; }
    }
}
