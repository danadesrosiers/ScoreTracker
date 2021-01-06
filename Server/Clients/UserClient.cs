using ScoreTracker.Server.Cosmos;
using ScoreTracker.Shared.Users;

namespace ScoreTracker.Server.Clients
{
    public class UserClient : CosmosClient<User>, IUserClient
    {
        public UserClient(CosmosCollectionFactory cosmosCollectionFactory) : base(cosmosCollectionFactory)
        {
        }
    }
}