using System;
using System.Runtime.Serialization;

namespace ScoreTracker.Shared.Meets
{
    [DataContract]
    public class MeetQuery
    {
        [DataMember(Order = 1)]
        public string Name { get; set; }
        [DataMember(Order = 2)]
        public DateTime? StartDate { get; set; }
        [DataMember(Order = 3)]
        public DateTime? EndDate { get; set; }
        [DataMember(Order = 4)]
        public int? Year { get; set; }
        [DataMember(Order = 5)]
        public StateCode? StateCode { get; set; }
        [DataMember(Order = 6)]
        public string City { get; set; }
        [DataMember(Order = 7)]
        public Discipline? Discipline { get; set; }
    }
}