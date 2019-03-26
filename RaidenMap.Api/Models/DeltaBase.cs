using MongoDB.Bson.Serialization.Attributes;
using System;

namespace RaidenMap.Api.Models
{
    public class DeltaBase
    {
        [BsonElement("timestamp")]
        public long Timestamp { get; set; }

        public override bool Equals(object obj)
        {
            return obj is DeltaBase @base &&
                   Timestamp == @base.Timestamp;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Timestamp);
        }
    }
}
