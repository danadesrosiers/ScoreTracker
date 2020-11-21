using System.Runtime.Serialization;

namespace ScoreTracker.Shared.Results
{
    [DataContract]
    public record Score
    {
        public Score(decimal eScore, decimal dScore, decimal neutralDeductions = default)
        {
            EScore = eScore;
            DScore = dScore;
            NeutralDeductions = neutralDeductions;
            FinalScore = (decimal) (eScore + dScore - NeutralDeductions);
        }

        public Score(decimal finalScore)
        {
            FinalScore = finalScore;
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
    }
}