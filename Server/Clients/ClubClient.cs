using ScoreTracker.Server.Cosmos;
using ScoreTracker.Shared.Clubs;

namespace ScoreTracker.Server.Clients
{
    public class ClubClient : CosmosClient<Club>, IClubClient
    {
        public ClubClient(CosmosCollectionFactory cosmosCollectionFactory) : base(cosmosCollectionFactory)
        {
        }
    }
}