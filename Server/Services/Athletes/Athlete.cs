using ScoreTracker.Server.Cosmos;
using ScoreTracker.Shared;

namespace ScoreTracker.Server.Services.Athletes
{
    public class Athlete : ICosmosEntity
    {
        public string Id { get; init; }
        public int ClubId { get; set; }
        public string Name { get; set; }
    }
}