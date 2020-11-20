using System.Collections.Generic;
using ScoreTracker.Shared.Results;

namespace ScoreTracker.Client.Services.RankStrategy
{
    public interface IRankStrategy
    {
        List<Result> AddRankings(List<Result> allScores);
    }
}