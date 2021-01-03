using ScoreTracker.Server.Cosmos;
using ScoreTracker.Shared.Users;

namespace ScoreTracker.Server.Clients
{
    public class UserClient : Client<User>, IUserClient
    {
        public UserClient(CosmosCollectionFactory cosmosCollectionFactory) : base(cosmosCollectionFactory)
        {
        }
    }
}