using System.Collections.Generic;
using ScoreTracker.Server.Services.Athletes;
using ScoreTracker.Server.Services.Clubs;
using Result = ScoreTracker.Server.Services.Results.Result;

namespace ScoreTracker.Server.Services.Meets
{
    public class MeetInfo
    {
        public Meet Meet { get; set; }
        public List<Athlete> Athletes { get; set; }
        public List<Club> Clubs { get; set; }
        public List<Result> Results { get; set; }
    }
}