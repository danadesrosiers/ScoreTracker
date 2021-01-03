using System.Collections.Generic;
using ScoreTracker.Server.Cosmos;
using ScoreTracker.Shared.Results;

namespace ScoreTracker.Server.Clients
{
    public class MeetResultClient : Client<MeetResult>, IMeetResultClient
    {
        public MeetResultClient(CosmosCollectionFactory cosmosCollectionFactory) : base(cosmosCollectionFactory)
        {
        }

        public IAsyncEnumerable<MeetResult> GetAsync(ResultsQuery query)
        {
            return base.GetAsync(query);
        }
    }
}