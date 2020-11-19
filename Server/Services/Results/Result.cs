using System.Collections.Generic;
using System.Linq;
using ScoreTracker.Server.Cosmos;
using ScoreTracker.Shared;

namespace ScoreTracker.Server.Services.Results
{
    public class Result : ICosmosEntity
    {
        private Score _allAroundScore;

        public string Id { get; init;}
        public int Season { get; set;}
        public int MeetId { get; set;}
        public string Session { get; set;}
        public string Level { get; set;}
        public string AgeGroup { get; set;}
        public int AthleteId { get; set;}
        public string AthleteName { get; set;}
        public int ClubId { get; set;}
        public string Club { get; set;}
        public Score Floor { get; set;}
        public Score Horse { get; set;}
        public Score Rings { get; set;}
        public Score Vault { get; set;}
        public Score PBars { get; set;}
        public Score HBar { get; set;}
        public string MeetIdLevelDivision { get; set;}
        public Score AllAround
        {
            get
            {
                if (_allAroundScore == null)
                {
                    var eventScores = new List<Score> {Floor, Horse, Rings, Vault, PBars, HBar};
                    var eScore = eventScores.Select(i => i?.EScore).Sum();
                    var dScore = eventScores.Select(i => i?.DScore).Sum();
                    var nd = eventScores.Select(i => i?.NeutralDeductions).Sum();
                    var finalScore = eventScores.Select(i => i?.FinalScore).Sum();

                    _allAroundScore = eScore + dScore > 0.0m ?
                        new Score(eScore.GetValueOrDefault(), dScore.GetValueOrDefault(), nd.GetValueOrDefault()) :
                        new Score(finalScore.GetValueOrDefault());
                }

                return _allAroundScore;
            }
            set => _allAroundScore = value;
        }
    }
}