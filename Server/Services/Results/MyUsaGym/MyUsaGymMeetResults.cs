using System;
using System.Collections.Generic;

namespace ScoreTracker.Server.Services.Results.MyUsaGym
{
    public record MyUsaGymMeetResults(IEnumerable<MyUsaGymScore> Scores);

    public record MyUsaGymScore(
        int ScoreId,
        int PersonId,
        int ClubId,
        string EventId,
        string SessionId,
        decimal? Difficulty,
        decimal? Execution,
        decimal? Deductions,
        decimal? FinalScore,
        string Place,
        int Rank,
        DateTime LastUpdate
    );
}