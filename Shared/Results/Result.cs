using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace ScoreTracker.Shared.Results
{
    [DataContract]
    public record Result : ICosmosEntity
    {
        private Score _allAroundScore;

        [DataMember(Order = 1)]
        public string Id { get; init;}
        [DataMember(Order = 2)]
        public int Season { get; set;}
        [DataMember(Order = 3)]
        public string MeetId { get; set;}
        [DataMember(Order = 4)]
        public string Session { get; set;}
        [DataMember(Order = 5)]
        public string Level { get; set;}
        [DataMember(Order = 6)]
        public string AgeGroup { get; set;}
        [DataMember(Order = 7)]
        public int AthleteId { get; set;}
        [DataMember(Order = 8)]
        public string AthleteName { get; set;}
        [DataMember(Order = 9)]
        public int ClubId { get; set;}
        [DataMember(Order = 10)]
        public string Club { get; set;}
        [DataMember(Order = 11)]
        public Score Floor { get; set;}
        [DataMember(Order = 12)]
        public Score Horse { get; set;}
        [DataMember(Order = 13)]
        public Score Rings { get; set;}
        [DataMember(Order = 14)]
        public Score Vault { get; set;}
        [DataMember(Order = 15)]
        public Score PBars { get; set;}
        [DataMember(Order = 16)]
        public Score HBar { get; set;}
        [DataMember(Order = 17)]
        public string MeetIdLevelDivision { get; set;}
        [DataMember(Order = 18)]
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