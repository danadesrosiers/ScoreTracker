using System;
using System.Runtime.Serialization;

namespace ScoreTracker.Shared.Subscriptions
{
    [DataContract]
    public record Subscription : ICosmosEntity
    {
        [DataMember(Order = 1)]
        public string Id { get; init; } = Guid.NewGuid().ToString();
        [DataMember(Order = 2)]
        public int ClubId { get; set; }
        [DataMember(Order = 3)]
        public int AthleteId { get; set; }
        [DataMember(Order = 4)]
        public string Name { get; set; }
        [DataMember(Order = 5)]
        public DateTime StartTime { get; set; }
    }
}