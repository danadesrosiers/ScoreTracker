using ScoreTracker.Shared.Results;

namespace ScoreTracker.Client.Services.RankStrategy;

public class BreakTiesRankStrategy: IRankStrategy
{
    public ICollection<MeetResult> SortResults(IEnumerable<MeetResult> results)
    {
        return (from result in results
            orderby result.AllAround?.FinalScore descending,
                HighScore(result, 1) descending,
                HighScore(result, 2) descending,
                HighScore(result, 3) descending,
                HighScore(result, 4) descending,
                HighScore(result, 5) descending,
                HighScore(result, 6) descending
            select result).ToList();
    }

    public ICollection<MeetResult> AddRankings(IEnumerable<MeetResult> results)
    {
        var timer = new Stopwatch();
        timer.Start();
        var sortedResults = SortResults(results);
        foreach (var (result, rank) in sortedResults.Select((value, i) => (value, i+1)))
        {
            result.AllAround.CombinedRank = rank;
        }

        var sortedScores = (from scores in sortedResults
            orderby scores.Floor?.FinalScore descending,
                scores.AllAround?.FinalScore descending
            select scores).ToList();
        foreach (var (result, rank) in sortedScores.Where(s => s.Floor != null).Select((value, i) => (value, i+1)))
        {
            result.Floor!.CombinedRank = rank;
        }

        sortedScores = (from scores in sortedResults
            orderby scores.Horse?.FinalScore descending,
                scores.AllAround?.FinalScore descending
            select scores).ToList();
        foreach (var (result, rank) in sortedScores.Where(s => s.Horse != null).Select((value, i) => (value, i+1)))
        {
            result.Horse!.CombinedRank = rank;
        }

        sortedScores = (from scores in sortedResults
            orderby scores.Rings?.FinalScore descending,
                scores.AllAround?.FinalScore descending
            select scores).ToList();
        foreach (var (result, rank) in sortedScores.Where(s => s.Rings != null).Select((value, i) => (value, i+1)))
        {
            result.Rings!.CombinedRank = rank;
        }

        sortedScores = (from scores in sortedResults
            orderby scores.Vault?.FinalScore descending,
                scores.AllAround?.FinalScore descending
            select scores).ToList();
        foreach (var (result, rank) in sortedScores.Where(s => s.Vault != null).Select((value, i) => (value, i+1)))
        {
            result.Vault!.CombinedRank = rank;
        }

        sortedScores = (from scores in sortedResults
            orderby scores.PBars?.FinalScore descending,
                scores.AllAround?.FinalScore descending
            select scores).ToList();
        foreach (var (result, rank) in sortedScores.Where(s => s.PBars != null).Select((value, i) => (value, i+1)))
        {
            result.PBars!.CombinedRank = rank;
        }

        sortedScores = (from scores in sortedResults
            orderby scores.HBar?.FinalScore descending,
                scores.AllAround?.FinalScore descending
            select scores).ToList();
        foreach (var (result, rank) in sortedScores.Where(s => s.HBar != null).Select((value, i) => (value, i+1)))
        {
            result.HBar!.CombinedRank = rank;
        }

        Console.WriteLine($"Added rankings in {timer.ElapsedMilliseconds}ms.");

        return sortedResults;
    }

    private decimal HighScore(MeetResult meetResult, int rank)
    {
        var eventScores = new List<Score>
        {
            meetResult.Floor ?? new Score(0),
            meetResult.Horse ?? new Score(0),
            meetResult.Rings ?? new Score(0),
            meetResult.Vault ?? new Score(0),
            meetResult.PBars ?? new Score(0),
            meetResult.HBar ?? new Score(0)
        };

        var sortedScores = from score in eventScores
            orderby score.FinalScore descending
            select score;

        return sortedScores.ElementAtOrDefault(rank)?.FinalScore ?? 0.0m;
    }
}