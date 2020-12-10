using System.Collections.Generic;
using ScoreTracker.Shared.Results;

namespace ScoreTracker.Client.Services.RankStrategy
{
    public interface IRankStrategy
    {
        ICollection<MeetResult> AddRankings(IEnumerable<MeetResult> sortedResults);
    }
}