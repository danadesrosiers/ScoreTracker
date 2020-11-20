using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ScoreTracker.Server.Services.Meets;
using ScoreTracker.Shared;
using ScoreTracker.Shared.Meets;
using ScoreTracker.Shared.Results;

namespace ScoreTracker.Server.Services.Results.MyUsaGym
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

        public async Task<MeetInfo> GetMeetInfoAsync(string meetId, bool getResults)
        {
            var usaGymMeet = await _httpClient.GetAsync<MyUsaGymMeet>(SanctionUri + meetId);

            var meetInfo = new MeetInfo
            {
                Meet = usaGymMeet.GetMeet(),
                Athletes = usaGymMeet.GetAthletes(),
                Clubs = usaGymMeet.GetClubs()
            };

            if (getResults)
            {
                meetInfo.Results = await usaGymMeet.SessionResultSets
                    .SelectManyAsync(resultSet => GetMeetResultsAsync(usaGymMeet, resultSet));
            }

            return meetInfo;
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
            try
            {
                meets.AddRange(await liveMeets);
                // meets.AddRange(await futureMeets);
                meets.AddRange(await pastMeets);
            }
            catch (Exception e)
            {
                var message = e.Message;
            }

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

        private async Task<IEnumerable<Result>> GetMeetResultsAsync(MyUsaGymMeet meet, SessionResultSet resultSet)
        {
            var scores = new Dictionary<int,Result>();
            var results = await _httpClient.GetAsync<MyUsaGymMeetResults>(ResultSetUri + resultSet.ResultSetId);
            foreach (var result in results.Scores)
            {
                if (!scores.ContainsKey(result.PersonId))
                {
                    var newScore = new Result
                    {
                        Id = $"{meet.Sanction.SanctionId}-{result.PersonId}-{result.SessionId}",
                        MeetId = Convert.ToInt32(meet.Sanction.SanctionId),
                        AthleteId = result.PersonId,
                        AthleteName = meet.People[result.PersonId].FirstName + " " + meet.People[result.PersonId].LastName,
                        ClubId = result.ClubId,
                        Club = meet.Clubs[result.ClubId].ShortName ?? meet.Clubs[result.ClubId].Name,
                        Level = resultSet.Level,
                        AgeGroup = resultSet.Division,
                        MeetIdLevelDivision = meet.Sanction.SanctionId + resultSet.Level + resultSet.Division,
                    };
                    scores[result.PersonId] = newScore;
                }

                switch (result.EventId)
                {
                    case "1":
                        scores[result.PersonId].Floor = new Score(result.FinalScore.GetValueOrDefault());
                        break;
                    case "2":
                        scores[result.PersonId].Horse = new Score(result.FinalScore.GetValueOrDefault());
                        break;
                    case "3":
                        scores[result.PersonId].Rings = new Score(result.FinalScore.GetValueOrDefault());
                        break;
                    case "4":
                        scores[result.PersonId].Vault = new Score(result.FinalScore.GetValueOrDefault());
                        break;
                    case "5":
                        scores[result.PersonId].PBars = new Score(result.FinalScore.GetValueOrDefault());
                        break;
                    case "6":
                        scores[result.PersonId].HBar = new Score(result.FinalScore.GetValueOrDefault());
                        break;
                }
            }

            return scores.Select(i => i.Value).ToList();
        }
    }
}