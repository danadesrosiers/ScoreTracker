using System;

namespace ScoreTracker.Server.Services.Results.MyUsaGym
{
    public class MyUsaGymMeetSearchResult
    {
        public string City { get; set; }
        public DateTime EndDate { get; set; }
        public bool HasMeetInfo { get; set; }
        public bool HasResults { get; set; }
        public string LogoUrl { get; set; }
        public string Name { get; set; }
        public Discipline Program { get; set; }
        public string SanctionId { get; set; }
        public string SiteName { get; set; }
        public DateTime StartDate { get; set; }
        public StateCode? State { get; set; }
        public string Website { get; set; }
    }
}