using MongoDB.Bson.Serialization.Attributes;

namespace RaidenMap.Api.Models
{
    public class Participant
    {
        [BsonElement("ethAddress")]
        public string EthAddress { get; set; }

        [BsonElement("deposit")]
        public long Deposit { get; set; }

        [BsonElement("withdrawnAmount")]
        public long WithdrawnAmount { get; set; }

        [BsonElement("wantsToClose")]
        public bool? WantsToClose { get; set; }
    }
}
