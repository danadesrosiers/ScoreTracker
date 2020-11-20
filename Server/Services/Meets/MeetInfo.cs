using System.Collections.Generic;
using ScoreTracker.Server.Services.Clubs;
using ScoreTracker.Shared.Athletes;
using ScoreTracker.Shared.Clubs;
using ScoreTracker.Shared.Meets;
using Result = ScoreTracker.Shared.Results.Result;

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