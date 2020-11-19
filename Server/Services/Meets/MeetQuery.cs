using System;

namespace ScoreTracker.Server.Services.Meets
{
    public class MeetQuery
    {
        public string Name { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? Year { get; set; }
        public StateCode? StateCode { get; set; }
        public string City { get; set; }
        public Discipline? Discipline { get; set; }
    }
}