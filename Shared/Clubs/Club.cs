using System.Runtime.Serialization;

namespace ScoreTracker.Shared.Clubs
{
    [DataContract]
    public record Club : CosmosEntity
    {
        [DataMember(Order = 1)]
        public override string? Id { get; init; }
        [DataMember(Order = 2)]
        public string Name { get; init; } = null!;
        [DataMember(Order = 3)]
        public string ShortName { get; init; } = null!;
        [DataMember(Order = 4)]
        public override string? ETag { get; init; }
    }
}