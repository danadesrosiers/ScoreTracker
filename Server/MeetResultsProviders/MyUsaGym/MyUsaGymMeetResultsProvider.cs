using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ScoreTracker.Shared;
using ScoreTracker.Shared.Meets;
using ScoreTracker.Shared.Results;

namespace ScoreTracker.Server.MeetResultsProviders.MyUsaGym
{
    public class MyUsaGymMeetResultsProvider : IMeetResultsProvider
    {
        private readonly HttpClient _httpClient;
        private const string SanctionUri = "https://api.myusagym.com/v2/sanctions/";
        private const string ResultSetUri = "https://api.myusagym.com/v2/resultsSets/";
        private const string PastMeets = "https://api.myusagym.com/v1/meets/past";
        private const string FutureMeets = "https://api.myusagym.com/v1/meets/future";
        private const string LiveMeets = "https://api.myusagym.com/v1/meets/live";

        public MyUsaGymMeetResultsProvider(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<MeetInfo> GetMeetInfoAsync(string meetId)
        {
            var usaGymMeet = await _httpClient.GetAsync<MyUsaGymMeet>(SanctionUri + meetId);

            return new MeetInfo
            {
                Meet = usaGymMeet.GetMeet(),
                Athletes = usaGymMeet.GetAthletes(),
                Clubs = usaGymMeet.GetClubs(),
                Results = await usaGymMeet.SessionResultSets
                    .SelectManyAsync(resultSet => GetMeetResultsAsync(usaGymMeet, resultSet))
            };
        }

        public async Task<Meet> GetMeetAsync(string meetId)
        {
            var usaGymMeet = await _httpClient.GetAsync<MyUsaGymMeet>(SanctionUri + meetId);
            return usaGymMeet.GetMeet();
        }

        public async Task<List<Meet>> SearchMeetsAsync(MeetQuery query)
        {
            var pastMeets = SearchMeetsAsync(query, PastMeets);
            // var futureMeets = SearchMeetsAsync(query, FutureMeets);
            var liveMeets = SearchMeetsAsync(query, LiveMeets);

            var meets = new List<Meet>();
            meets.AddRange(await liveMeets);
            // meets.AddRange(await futureMeets);
            meets.AddRange(await pastMeets);

            return meets;
        }

        private async Task<List<Meet>> SearchMeetsAsync(MeetQuery query, string url)
        {
            var meets = await _httpClient.GetAsync<List<MyUsaGymMeetSearchResult>>(url);

            return (from meet in meets.TakeWhile(meet => query.StartDate == null || meet.StartDate >= query.StartDate)
                where (query.Year == null || query.Year == meet.StartDate.Year) &&
                      (query.EndDate == null || query.EndDate >= meet.EndDate) &&
                      (query.Discipline == Discipline.All || query.Discipline == meet.Program) &&
                      (query.StateCode == StateCode.Any || query.StateCode == meet.State) &&
                      (query.Name == null || meet.Name.ToLower().Contains(query.Name.ToLower()))
                select new Meet
                {
                    Id = meet.SanctionId,
                    Name = meet.Name,
                    StartDate = DateTime.SpecifyKind(meet.StartDate, DateTimeKind.Utc),
                    EndDate = DateTime.SpecifyKind(meet.EndDate, DateTimeKind.Utc),
                    Season = meet.StartDate.Year,
                    State = meet.State,
                    Discipline = meet.Program,
                }).ToList();
        }

        private async Task<IEnumerable<MeetResult>> GetMeetResultsAsync(MyUsaGymMeet meet, SessionResultSet resultSet)
        {
            var results = new Dictionary<string,MeetResult>();
            var uri = ResultSetUri + resultSet.ResultSetId;
            foreach (var score in (await _httpClient.GetAsync<MyUsaGymMeetResults>(uri)).Scores)
            {
                if (!results.ContainsKey(score.PersonId))
                {
                    var newScore = new MeetResult
                    {
                        Id = $"{meet.Sanction.SanctionId}-{score.PersonId}-{score.SessionId}",
                        MeetId = meet.Sanction.SanctionId,
                        AthleteId = score.PersonId,
                        AthleteName = meet.People[score.PersonId].FirstName + " " + meet.People[score.PersonId].LastName,
                        ClubId = score.ClubId,
                        Club = meet.Clubs[score.ClubId].ShortName ?? meet.Clubs[score.ClubId].Name,
                        Level = resultSet.Level,
                        AgeGroup = resultSet.Division,
                        MeetIdLevelDivision = meet.Sanction.SanctionId + resultSet.Level + resultSet.Division
                    };
                    results[score.PersonId] = newScore;
                }

                switch (score.EventId)
                {
                    case "1":
                        results[score.PersonId].Floor = new Score(score.FinalScore.GetValueOrDefault(), score.Rank, score.LastUpdate);
                        break;
                    case "2":
                        results[score.PersonId].Horse = new Score(score.FinalScore.GetValueOrDefault(), score.Rank, score.LastUpdate);
                        break;
                    case "3":
                        results[score.PersonId].Rings = new Score(score.FinalScore.GetValueOrDefault(), score.Rank, score.LastUpdate);
                        break;
                    case "4":
                        results[score.PersonId].Vault = new Score(score.FinalScore.GetValueOrDefault(), score.Rank, score.LastUpdate);
                        break;
                    case "5":
                        results[score.PersonId].PBars = new Score(score.FinalScore.GetValueOrDefault(), score.Rank, score.LastUpdate);
                        break;
                    case "6":
                        results[score.PersonId].HBar = new Score(score.FinalScore.GetValueOrDefault(), score.Rank, score.LastUpdate);
                        break;
                    case "aa":
                        results[score.PersonId].AllAround = new Score(score.FinalScore.GetValueOrDefault(), score.Rank, score.LastUpdate);
                        break;
                }
            }

            return results.Select(i => i.Value).ToList();
        }
    }
}