using System.Runtime.Serialization;

namespace ScoreTracker.Shared.Athletes
{
    [DataContract]
    public record Athlete : ICosmosEntity
    {
        [DataMember(Order = 1)]
        public string Id { get; init; }
        [DataMember(Order = 2)]
        public int ClubId { get; set; }
        [DataMember(Order = 3)]
        public string Name { get; set; }
    }
}