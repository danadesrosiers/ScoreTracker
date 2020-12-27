using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace ScoreTracker.Shared.Results
{
    [DataContract]
    public record MeetResult : CosmosEntity
    {
        private Score? _allAroundScore;

        [DataMember(Order = 1)]
        public override string? Id { get; init;}
        [DataMember(Order = 2)]
        public int Season { get; init; }
        [DataMember(Order = 3)]
        public string MeetId { get; init; } = null!;
        [DataMember(Order = 4)]
        public string Session { get; init; } = null!;
        [DataMember(Order = 5)]
        public string Level { get; init; } = null!;
        [DataMember(Order = 6)]
        public string AgeGroup { get; init; } = null!;
        [DataMember(Order = 7)]
        public string AthleteId { get; init; } = null!;
        [DataMember(Order = 8)]
        public string AthleteName { get; init; } = null!;
        [DataMember(Order = 9)]
        public string ClubId { get; init; } = null!;
        [DataMember(Order = 10)]
        public string Club { get; init; } = null!;
        [DataMember(Order = 11)]
        public Score? Floor { get; set; }
        [DataMember(Order = 12)]
        public Score? Horse { get; set; }
        [DataMember(Order = 13)]
        public Score? Rings { get; set; }
        [DataMember(Order = 14)]
        public Score? Vault { get; set; }
        [DataMember(Order = 15)]
        public Score? PBars { get; set; }
        [DataMember(Order = 16)]
        public Score? HBar { get; set; }
        [DataMember(Order = 17)]
        public string MeetIdLevelDivision { get; init; } = null!;
        [DataMember(Order = 18)]
        public Score AllAround
        {
            get
            {
                if (_allAroundScore == null)
                {
                    var eventScores = new List<Score?> {Floor, Horse, Rings, Vault, PBars, HBar};
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

        [DataMember(Order = 19)]
        public DateTime? LastUpdated
        {
            get =>
                new List<DateTime?>
                {
                    Floor?.LastModified,
                    Horse?.LastModified,
                    Rings?.LastModified,
                    Vault?.LastModified,
                    PBars?.LastModified,
                    HBar?.LastModified
                }.Max();
            set { }
        }
        [DataMember(Order = 20)]
        public override string? ETag { get; init; }
    }
}