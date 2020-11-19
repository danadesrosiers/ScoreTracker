namespace ScoreTracker.Server.Services.Results
{
    public class Score
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

        public decimal? EScore { get; set; }
        public decimal? DScore { get; set; }
        public decimal? NeutralDeductions { get; set; }
        public decimal FinalScore { get; set; }
        public int? Rank { get; set; }
    }
}