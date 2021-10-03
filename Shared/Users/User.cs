namespace ScoreTracker.Shared.Users;

[DataContract]
public record User : CosmosEntity
{
    [DataMember(Order = 1)]
    public override string? Id { get; init; } = Guid.NewGuid().ToString();
    [DataMember(Order = 2)]
    public string Name { get; init; } = null!;
    [DataMember(Order = 3)]
    public ICollection<Subscription> Subscriptions { get; init; } = new List<Subscription>();
    [DataMember(Order = 4)]
    public override string? ETag { get; init; }
}

[DataContract]
public record Subscription
{
    [DataMember(Order = 1)]
    public string? ClubId { get; init; }
    [DataMember(Order = 2)]
    public string? AthleteId { get; init; }
    [DataMember(Order = 3)]
    public string Name { get; init; } = null!;
    [DataMember(Order = 4)]
    public DateTime StartTime { get; init; }
    [DataMember(Order = 5)]
    public Discipline? Discipline { get; init; }
    [DataMember(Order = 6)]
    public string? Level { get; init; }
    [DataMember(Order = 7)]
    public bool ReceiveNotifications { get; set; }
}