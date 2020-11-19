using System.Collections.Generic;
using System.Linq;
using ScoreTracker.Shared;

namespace ScoreTracker.Client.Services.RankStrategy
{
    public class BreakTiesRankStrategy: IRankStrategy
    {
        public List<Result> AddRankings(List<Result> allScores)
        {
            allScores = (from scores in allScores
                orderby (decimal)scores.AllAround?.FinalScore descending,
                    HighScore(scores, 1) descending,
                    HighScore(scores, 2) descending,
                    HighScore(scores, 3) descending,
                    HighScore(scores, 4) descending,
                    HighScore(scores, 5) descending,
                    HighScore(scores, 6) descending
                select scores).ToList();
            foreach (var (result, rank) in allScores.Where(s => s.AllAround != null).Select((value, i) => (value, i+1)))
            {
                if (result.AllAround != null)
                {
                    result.AllAround.Rank = rank;
                }
            }

            var sortedScores = (from scores in allScores
                orderby (decimal)scores.Floor?.FinalScore descending,
                        (decimal)scores.AllAround?.FinalScore descending
                select scores).ToList();
            foreach (var (result, rank) in sortedScores.Where(s => s.Floor != null).Select((value, i) => (value, i+1)))
            {
                result.Floor.Rank = rank;
            }

            sortedScores = (from scores in allScores
                orderby (decimal)scores.Horse?.FinalScore descending,
                        (decimal)scores.AllAround?.FinalScore descending
                select scores).ToList();
            foreach (var (result, rank) in sortedScores.Where(s => s.Horse != null).Select((value, i) => (value, i+1)))
            {
                result.Horse.Rank = rank;
            }

            sortedScores = (from scores in allScores
                orderby (decimal)scores.Rings?.FinalScore descending,
                        (decimal)scores.AllAround?.FinalScore descending
                select scores).ToList();
            foreach (var (result, rank) in sortedScores.Where(s => s.Rings != null).Select((value, i) => (value, i+1)))
            {
                result.Rings.Rank = rank;
            }

            sortedScores = (from scores in allScores
                orderby (decimal)scores.Vault?.FinalScore descending,
                        (decimal)scores.AllAround?.FinalScore descending
                select scores).ToList();
            foreach (var (result, rank) in sortedScores.Where(s => s.Vault != null).Select((value, i) => (value, i+1)))
            {
                result.Vault.Rank = rank;
            }

            sortedScores = (from scores in allScores
                orderby (decimal)scores.PBars?.FinalScore descending,
                        (decimal)scores.AllAround?.FinalScore descending
                select scores).ToList();
            foreach (var (result, rank) in sortedScores.Where(s => s.PBars != null).Select((value, i) => (value, i+1)))
            {
                result.PBars.Rank = rank;
            }

            sortedScores = (from scores in allScores
                orderby (decimal)scores.HBar?.FinalScore descending,
                        (decimal)scores.AllAround?.FinalScore descending
                select scores).ToList();
            foreach (var (result, rank) in sortedScores.Where(s => s.HBar != null).Select((value, i) => (value, i+1)))
            {
                result.HBar.Rank = rank;
            }

            return allScores;
        }

        private decimal HighScore(Result result, int rank)
        {
            var eventScores = new List<Score>
            {
                result.Floor,
                result.Horse,
                result.Rings,
                result.Vault,
                result.PBars,
                result.HBar
            };

            var sortedScores = from score in eventScores
                orderby (decimal)score.FinalScore descending
                select score;

            return sortedScores.ElementAtOrDefault(rank)?.FinalScore ?? 0.0m;
        }
    }
}