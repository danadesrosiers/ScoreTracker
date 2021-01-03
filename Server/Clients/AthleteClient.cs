using System.Collections.Generic;
using ScoreTracker.Server.Cosmos;
using ScoreTracker.Shared.Athletes;

namespace ScoreTracker.Server.Clients
{
    public class AthleteClient : Client<Athlete>, IAthleteClient
    {
        public AthleteClient(CosmosCollectionFactory cosmosCollectionFactory) : base(cosmosCollectionFactory)
        {
        }

        public IAsyncEnumerable<AthleteResult> GetAsync(AthleteResultQuery query)
        {
            return base.GetAsync(query);
        }
    }
}