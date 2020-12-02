using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ScoreTracker.Shared.Users
{
    [DataContract]
    public record User : ICosmosEntity
    {
        [DataMember(Order = 1)]
        public string Id { get; init; } = Guid.NewGuid().ToString();
        [DataMember(Order = 2)]
        public string Name { get; init; }
        [DataMember(Order = 3)]
        public ICollection<Subscription> Subscriptions { get; init; } = new List<Subscription>();

        [DataContract]
        public record Subscription
        {
            [DataMember(Order = 1)]
            public string ClubId { get; init; }
            [DataMember(Order = 2)]
            public string AthleteId { get; init; }
            [DataMember(Order = 3)]
            public string Name { get; init; }
            [DataMember(Order = 4)]
            public DateTime StartTime { get; init; }
            // TODO: Discipline
            // TODO: Notification preferences.
        }
    }
}