using ScoreTracker.Shared.Results;

namespace ScoreTracker.Shared.Athletes;

[DataContract]
public record Athlete : CosmosEntity
{
    [DataMember(Order = 1)]
    public override string? Id { get; init; }
    [DataMember(Order = 2)]
    public ICollection<AthleteResult> RecentScores { get; init; } = new List<AthleteResult>();
    public string? Name => RecentScores.FirstOrDefault()?.AthleteName;
    public string? ClubId => RecentScores.FirstOrDefault()?.ClubId;
    public string? ClubName => RecentScores.FirstOrDefault()?.ClubName;
    public override string? ETag { get; init; }
}

[DataContract]
public record AthleteResult
{
    [DataMember(Order = 1)]
    public string MeetId { get; init; } = null!;
    [DataMember(Order = 2)]
    public string MeetName { get; init; } = null!;
    [DataMember(Order = 3)]
    public string ResultId { get; init; } = null!;
    [DataMember(Order = 4)]
    public Discipline Discipline { get; init; }
    [DataMember(Order = 5)]
    public Event Event { get; init; }
    [DataMember(Order = 6)]
    public Score Score { get; init; } = null!;
    [DataMember(Order = 7)]
    public string AthleteId { get; init; } = null!;
    [DataMember(Order = 8)]
    public string AthleteName { get; init; } = null!;
    [DataMember(Order = 9)]
    public string ClubId { get; init; } = null!;
    [DataMember(Order = 10)]
    public string ClubName { get; init; } = null!;
    [DataMember(Order = 11)]
    public string Level { get; set; } = null!;
}

public enum Event
{
    FX,
    PH,
    SR,
    VT,
    PB,
    HB
}