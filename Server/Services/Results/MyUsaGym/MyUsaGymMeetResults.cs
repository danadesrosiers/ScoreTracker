using System.Collections.Generic;
using System.Linq;
using ScoreTracker.Server.Services.Clubs;
using ScoreTracker.Shared;
using ScoreTracker.Shared.Clubs;
using ScoreTracker.Shared.Results;

namespace ScoreTracker.Server.Services.Results.MyUsaGym
{
    public record MyUsaGymMeetResults(IEnumerable<MyUsaGymScore> Scores);

    public record MyUsaGymScore(
        int PersonId,
        int ClubId,
        string EventId,
        string SessionId,
        decimal? Difficulty,
        decimal? Execution,
        decimal? Deductions,
        decimal? FinalScore
    );
}