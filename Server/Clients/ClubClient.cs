namespace ScoreTracker.Server.Clients;

public class ClubClient : CosmosClient<Club>, IClubClient
{
    public ClubClient(CosmosCollectionFactory cosmosCollectionFactory) : base(cosmosCollectionFactory)
    {
    }
}