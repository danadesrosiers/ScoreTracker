using System.Collections.Generic;
using ScoreTracker.Shared.Athletes;
using ScoreTracker.Shared.Clubs;
using ScoreTracker.Shared.Meets;
using ScoreTracker.Shared.Results;

namespace ScoreTracker.Server.MeetResultsProviders.MyUsaGym
{
    public class MeetInfo
    {
        public Meet Meet { get; init; } = null!;
        public List<Athlete> Athletes { get; init; } = new();
        public List<Club> Clubs { get; init; } = new();
        public List<MeetResult>? Results { get; init; }
    }
}