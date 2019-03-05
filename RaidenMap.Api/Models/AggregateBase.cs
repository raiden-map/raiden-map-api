using MongoDB.Bson.Serialization.Attributes;
using System;

namespace RaidenMap.Api.Models
{
    public class AggregateBase
    {
        [BsonElement("timestamp")]
        public long Timestamp { get; set; }

        public override bool Equals(object obj)
        {
            return obj is AggregateBase @base &&
                   Timestamp == @base.Timestamp;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Timestamp);
        }
    }
}
