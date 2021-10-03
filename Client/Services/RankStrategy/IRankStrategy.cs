namespace ScoreTracker.Client.Services.RankStrategy;

public interface IRankStrategy
{
    ICollection<MeetResult> AddRankings(IEnumerable<MeetResult> sortedResults);
}