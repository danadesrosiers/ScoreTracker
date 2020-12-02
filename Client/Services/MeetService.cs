using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ScoreTracker.Client.Services.RankStrategy;
using ScoreTracker.Shared;
using ScoreTracker.Shared.Meets;
using ScoreTracker.Shared.Results;
using ScoreTracker.Shared.Users;

namespace ScoreTracker.Client.Services
{
    public class MeetService
    {
        private readonly IMeetService _meetClient;
        private readonly IResultService _resultsClient;
        private readonly IRankStrategy _ranker;

        public MeetService(IMeetService meetClient, IResultService resultsClient, IRankStrategy ranker)
        {
            _meetClient = meetClient;
            _resultsClient = resultsClient;
            _ranker = ranker;
        }

        public async Task<Meet> GetMeetAsync(string meetId)
        {
            return await _meetClient.GetAsync(meetId);
        }

        public async Task<Dictionary<string, string>> SearchMeetsAsync(int selectedSeason, StateCode? selectedState,
            Discipline? selectedDiscipline, string searchString)
        {
            var meetQuery = new MeetQuery
            {
                StateCode = selectedState,
                Year = selectedSeason,
                Discipline = selectedDiscipline,
                Name = searchString
            };
            return await _meetClient
                .SearchAsync(meetQuery)
                .ToDictionaryAsync(m => m.Id, m => m.Name);
        }

        public async Task<ICollection<Meet>> GetFollowingMeetsAsync(User user)
        {
            // TODO: Need to get only meets for the current user.  Query by Athletes and Clubs in Subscriptions.
            return await _meetClient.SearchAsync(new MeetQuery()).ToListAsync();
        }

        public async Task<ICollection<Result>> GetResults(string meetId, IEnumerable<string> divisions)
        {
            var timer = new Stopwatch();
            timer.Start();
            var query = new ResultsQuery { MeetId = meetId, Divisions = divisions };
            var results = await _resultsClient.GetAsync(query).ToListAsync();
            Console.WriteLine($"Found {results.Count} Results in {timer.ElapsedMilliseconds}ms.");
            return _ranker.AddRankings(results);
        }

        public ICollection<Result> CalculateTeamResults(IEnumerable<Result> results)
        {
            var timer = new Stopwatch();
            timer.Start();
            var scoresByClubLevel = new Dictionary<string, List<Result>>();
            foreach (var result in results)
            {
                var key = result.ClubId + result.Level;
                if (!scoresByClubLevel.ContainsKey(key))
                {
                    scoresByClubLevel[key] = new List<Result>();
                }

                scoresByClubLevel[key].Add(result);
            }

            var teamScores = new List<Result>();
            foreach (var (_, clubScores) in scoresByClubLevel)
            {
                var floorScore = (from result in clubScores
                    orderby result.Floor.FinalScore descending
                    select result.Floor.FinalScore).Take(3).Sum();
                var horseScore = (from result in clubScores
                    orderby result.Horse.FinalScore descending
                    select result.Horse.FinalScore).Take(3).Sum();
                var ringsScore = (from result in clubScores
                    orderby result.Rings.FinalScore descending
                    select result.Rings.FinalScore).Take(3).Sum();
                var vaultScore = (from result in clubScores
                    orderby result.Vault.FinalScore descending
                    select result.Vault.FinalScore).Take(3).Sum();
                var pBarsScore = (from result in clubScores
                    orderby result.PBars.FinalScore descending
                    select result.PBars.FinalScore).Take(3).Sum();
                var hBarScore = (from result in clubScores
                    orderby result.HBar.FinalScore descending
                    select result.HBar.FinalScore).Take(3).Sum();

                teamScores.Add(new Result
                {
                    Club = clubScores[0].Club,
                    ClubId = clubScores[0].ClubId,
                    Level = clubScores[0].Level,
                    Floor = new Score(floorScore),
                    Horse = new Score(horseScore),
                    Rings = new Score(ringsScore),
                    Vault = new Score(vaultScore),
                    PBars = new Score(pBarsScore),
                    HBar = new Score(hBarScore),
                    AllAround = new Score(floorScore + horseScore + ringsScore + vaultScore + pBarsScore + hBarScore)
                });
            }

            Console.WriteLine($"Calculated Team Scores in {timer.ElapsedMilliseconds}ms.");

            return _ranker.AddRankings(teamScores);
        }
    }
}