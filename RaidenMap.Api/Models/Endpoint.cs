using MongoDB.Bson.Serialization.Attributes;
using System;

namespace RaidenMap.Api.Models
{
    public class Endpoint
    {
        [BsonElement("ethAddress")]
        public string EthAddress { get; set; }

        [BsonElement("ipAddress")]
        public string IpAddress { get; set; }

        [BsonElement("state")]
        public string State { get; set; }

        [BsonElement("latitude")]
        public double Latitude { get; set; }

        [BsonElement("longitude")]
        public double Longitude { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Endpoint endpoint &&
                   EthAddress == endpoint.EthAddress &&
                   IpAddress == endpoint.IpAddress &&
                   State == endpoint.State &&
                   Latitude == endpoint.Latitude &&
                   Longitude == endpoint.Longitude;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(EthAddress, IpAddress, State, Latitude, Longitude);
        }
    }
}
