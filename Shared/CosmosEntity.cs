namespace ScoreTracker.Shared;

public abstract record CosmosEntity
{
    public abstract string? Id { get; init; }
    [JsonPropertyName("_etag")]
    public abstract string? ETag { get; init; }

    // Overriding the Equality functionality to ignore ETag.
    public virtual bool Equals(CosmosEntity? other)
    {
        return !(other is null) &&
               EqualityContract == other.EqualityContract &&
               EqualityComparer<string>.Default.Equals(Id, other.Id);
    }

    public override int GetHashCode()
    {
        return EqualityComparer<Type>.Default.GetHashCode(EqualityContract) +
               EqualityComparer<string>.Default.GetHashCode(Id ?? "");
    }
}