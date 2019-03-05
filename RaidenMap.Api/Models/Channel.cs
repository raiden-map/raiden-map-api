using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

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

        public override bool Equals(object obj)
        {
            return obj is Channel channel &&
                   ChannelId == channel.ChannelId &&
                   State == channel.State &&
                   LastStateChangeBlock == channel.LastStateChangeBlock &&
                   SettleTimeout == channel.SettleTimeout &&
                   EqualityComparer<Participant>.Default.Equals(FirstParticipant, channel.FirstParticipant) &&
                   EqualityComparer<Participant>.Default.Equals(SecondParticipant, channel.SecondParticipant);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ChannelId, State, LastStateChangeBlock, SettleTimeout, FirstParticipant, SecondParticipant);
        }
    }
}
