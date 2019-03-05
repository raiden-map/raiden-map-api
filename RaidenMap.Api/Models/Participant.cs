using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

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

        public override bool Equals(object obj)
        {
            return obj is Participant participant &&
                   EthAddress == participant.EthAddress &&
                   Deposit == participant.Deposit &&
                   WithdrawnAmount == participant.WithdrawnAmount &&
                   EqualityComparer<bool?>.Default.Equals(WantsToClose, participant.WantsToClose);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(EthAddress, Deposit, WithdrawnAmount, WantsToClose);
        }
    }
}
