using MongoDB.Bson.Serialization.Attributes;
using System;

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

        public override bool Equals(object obj)
        {
            return obj is Token token &&
                   Name == token.Name &&
                   Tag == token.Tag &&
                   ImageUrl == token.ImageUrl &&
                   ValueUsd == token.ValueUsd &&
                   ValueEth == token.ValueEth &&
                   ValueBtc == token.ValueBtc &&
                   PriceChangeDayUsd == token.PriceChangeDayUsd &&
                   PriceChangeWeekUsd == token.PriceChangeWeekUsd &&
                   PriceChangeDayEth == token.PriceChangeDayEth &&
                   PriceChangeWeekEth == token.PriceChangeWeekEth &&
                   PriceChangeDayBtc == token.PriceChangeDayBtc &&
                   PriceChangeWeekBtc == token.PriceChangeWeekBtc &&
                   MarketCap == token.MarketCap &&
                   Volume == token.Volume &&
                   Timestamp == token.Timestamp;
        }

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(Name);
            hash.Add(Tag);
            hash.Add(ImageUrl);
            hash.Add(ValueUsd);
            hash.Add(ValueEth);
            hash.Add(ValueBtc);
            hash.Add(PriceChangeDayUsd);
            hash.Add(PriceChangeWeekUsd);
            hash.Add(PriceChangeDayEth);
            hash.Add(PriceChangeWeekEth);
            hash.Add(PriceChangeDayBtc);
            hash.Add(PriceChangeWeekBtc);
            hash.Add(MarketCap);
            hash.Add(Volume);
            hash.Add(Timestamp);
            return hash.ToHashCode();
        }
    }
}
