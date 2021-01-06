using System.Collections.Generic;
using ScoreTracker.Server.Cosmos;
using ScoreTracker.Shared.Meets;

namespace ScoreTracker.Server.Clients
{
    public class MeetClient : CosmosClient<Meet>, IMeetClient
    {
        public MeetClient(CosmosCollectionFactory cosmosCollectionFactory) : base(cosmosCollectionFactory)
        {
        }

        public IAsyncEnumerable<Meet> GetAsync(MeetQuery query)
        {
            return base.GetAsync(query);
        }
    }
}