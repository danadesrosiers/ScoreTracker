using System.Collections.Generic;
using System.Runtime.Serialization;
using ScoreTracker.Shared.Results;

namespace ScoreTracker.Shared.Athletes
{
    [DataContract]
    public record Athlete : ICosmosEntity
    {
        [DataMember(Order = 1)]
        public string Id { get; init; }
        [DataMember(Order = 2)]
        public int ClubId { get; set; }
        [DataMember(Order = 3)]
        public string Name { get; set; }
        [DataMember(Order = 4)]
        public IList<AthleteScore> RecentScores { get; set; }
    }

    public record AthleteScore
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
        public string CLubName { get; init; }
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