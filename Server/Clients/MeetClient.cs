namespace ScoreTracker.Server.Clients;

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