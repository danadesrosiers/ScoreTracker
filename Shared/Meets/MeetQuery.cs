using System;
using System.Runtime.Serialization;

namespace ScoreTracker.Shared.Meets
{
    [DataContract]
    public record MeetQuery
    {
        [DataMember(Order = 1)]
        public string Name { get; init; }
        [DataMember(Order = 2)]
        public DateTime? StartDate { get; init; }
        [DataMember(Order = 3)]
        public DateTime? EndDate { get; init; }
        [DataMember(Order = 4)]
        public int? Year { get; init; }
        [DataMember(Order = 5)]
        public StateCode? StateCode { get; init; }
        [DataMember(Order = 6)]
        public string City { get; init; }
        [DataMember(Order = 7)]
        public Discipline? Discipline { get; init; }
    }
}