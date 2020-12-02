using System.Runtime.Serialization;

namespace ScoreTracker.Shared.Results
{
    [DataContract]
    public record Score
    {
        public Score(decimal eScore, decimal dScore, decimal neutralDeductions, int? rank = null)
        {
            EScore = eScore;
            DScore = dScore;
            NeutralDeductions = neutralDeductions;
            FinalScore = (decimal) (eScore + dScore - NeutralDeductions);
            Rank = rank;
        }

        public Score(decimal finalScore, int? rank = null)
        {
            FinalScore = finalScore;
            Rank = rank;
        }

        private Score()
        {
        }

        [DataMember(Order = 1)]
        public decimal? EScore { get; init; }
        [DataMember(Order = 2)]
        public decimal? DScore { get; init; }
        [DataMember(Order = 3)]
        public decimal? NeutralDeductions { get; init; }
        [DataMember(Order = 4)]
        public decimal FinalScore { get; init; }
        [DataMember(Order = 5)]
        public int? Rank { get; set; }
        public int? CombinedRank { get; set; }
    }
}