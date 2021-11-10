using ScoreTracker.Server.Cosmos;
using ScoreTracker.Shared.Results;

namespace ScoreTracker.Server.Clients;

public class MeetResultClient : CosmosClient<MeetResult>, IMeetResultClient
{
    public MeetResultClient(CosmosCollectionFactory cosmosCollectionFactory) : base(cosmosCollectionFactory)
    {
    }

    public IAsyncEnumerable<MeetResult> GetAsync(ResultsQuery query)
    {
        return base.GetAsync(query);
    }
}