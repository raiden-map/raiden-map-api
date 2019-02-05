using MongoDB.Bson.Serialization.Attributes;

namespace RaidenMap.Api.Models
{
    public class Channel
    {
        [BsonElement("channelId")]
        public long ChannelId { get; set; }

        [BsonElement("state")]
        public string State { get; set; }

        [BsonElement("lastStateChangeBlock")]
        public long LastStateChangeBlock { get; set; }

        [BsonElement("settleTimeout")]
        public long SettleTimeout { get; set; }

        [BsonElement("firstParticipant")]
        public Participant FirstParticipant { get; set; }

        [BsonElement("secondParticipant")]
        public Participant SecondParticipant { get; set; }

    }
}
