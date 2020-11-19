using System.Collections.Generic;
using ScoreTracker.Shared;

namespace ScoreTracker.Client.Services.RankStrategy
{
    public interface IRankStrategy
    {
        List<Result> AddRankings(List<Result> allScores);
    }
}