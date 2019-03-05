using MongoDB.Bson.Serialization.Attributes;

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
    }
}
