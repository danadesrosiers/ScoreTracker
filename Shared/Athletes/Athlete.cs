using System.Collections.Generic;
using System.Runtime.Serialization;
using ScoreTracker.Shared.Results;

namespace ScoreTracker.Shared.Athletes
{
    [DataContract]
    public record Athlete : CosmosEntity
    {
        [DataMember(Order = 1)]
        public override string Id { get; init; }
        [DataMember(Order = 2)]
        public string ClubId { get; init; }
        [DataMember(Order = 3)]
        public string Name { get; init; }
        [DataMember(Order = 4)]
        public ICollection<AthleteResult> RecentScores { get; init; }
        [DataMember(Order = 5)]
        public override string ETag { get; init; }
    }

    [DataContract]
    public record AthleteResult
    {
        [DataMember(Order = 1)]
        public string MeetId { get; init; }
        [DataMember(Order = 2)]
        public string MeetName { get; init; }
        [DataMember(Order = 3)]
        public string ResultId { get; init; }
        [DataMember(Order = 4)]
        public Discipline Discipline { get; init; }
        [DataMember(Order = 5)]
        public Event Event { get; init; }
        [DataMember(Order = 6)]
        public Score Score { get; init; }
        [DataMember(Order = 7)]
        public string AthleteId { get; init; }
        [DataMember(Order = 8)]
        public string AthleteName { get; init; }
        [DataMember(Order = 9)]
        public string ClubId { get; init; }
        [DataMember(Order = 10)]
        public string ClubName { get; init; }
    }

    public enum Event
    {
        FX,
        PH,
        SR,
        VT,
        PB,
        HB
    }
}