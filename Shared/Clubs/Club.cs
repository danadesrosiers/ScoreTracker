using System.Runtime.Serialization;

namespace ScoreTracker.Shared.Clubs
{
    [DataContract]
    public class Club : ICosmosEntity
    {
        [DataMember(Order = 1)]
        public string Id { get; init; }
        [DataMember(Order = 2)]
        public string Name { get; set; }
        [DataMember(Order = 3)]
        public string ShortName { get; set; }
    }
}