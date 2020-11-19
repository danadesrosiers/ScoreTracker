using ScoreTracker.Server.Cosmos;
using ScoreTracker.Shared;

namespace ScoreTracker.Server.Services.Clubs
{
    public class Club : ICosmosEntity
    {
        public string Id { get; init; }
        public string Name { get; set; }
        public string ShortName { get; set; }
    }
}