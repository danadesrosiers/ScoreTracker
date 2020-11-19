using System.Collections.Generic;
using System.Linq;
using ScoreTracker.Server.Services.Clubs;
using ScoreTracker.Shared;

namespace ScoreTracker.Server.Services.Results.MyUsaGym
{
    public class MyUsaGymMeetResults
    {
        public List<MyUsaGymScore> Scores { get; set; }

        public List<Result> GetAthleteResults(int meetId, string level, string division, List<MyUsaGymPerson> people, List<Club> clubs)
        {
            var scores = new Dictionary<int,Result>();
            foreach (var result in Scores)
            {
                if (!scores.ContainsKey(result.PersonId))
                {
                    var newScore = new Result
                    {
                        MeetId = meetId,
                        AthleteName = people[result.PersonId].FirstName + " " + people[result.PersonId].LastName,
                        Club = clubs[result.ClubId].ShortName ?? clubs[result.ClubId].Name,
                        Level = level,
                        AgeGroup = division,
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

    public class MyUsaGymScore
    {
        public int PersonId { get; set; }
        public int ClubId { get; set; }
        public string EventId { get; set; }
        public string SessionId { get; set; }
        public decimal? Difficulty { get; set; }
        public decimal? Execution { get; set; }
        public decimal? Deductions { get; set; }
        public decimal? FinalScore { get; set; }
    }
}