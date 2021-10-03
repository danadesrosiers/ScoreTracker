namespace ScoreTracker.Shared.Results;

[DataContract]
public record Score
{
    public Score(decimal eScore, decimal dScore, decimal neutralDeductions, int? rank = null, DateTime? lastModified = null)
    {
        EScore = eScore;
        DScore = dScore;
        NeutralDeductions = neutralDeductions;
        FinalScore = eScore + dScore - NeutralDeductions ?? default;
        Rank = rank;
        LastModified = lastModified;
    }

    public Score(decimal finalScore, int? rank = null, DateTime? lastModified = null)
    {
        FinalScore = finalScore;
        Rank = rank;
        LastModified = lastModified;
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
    public int? Rank { get; init; }
    [DataMember(Order = 6)]
    public DateTime? LastModified { get; init; }
    public int? CombinedRank { get; set; }
}