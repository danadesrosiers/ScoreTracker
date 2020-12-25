using System.Collections.Generic;
using System.Threading.Tasks;
using ScoreTracker.Shared.Athletes;

namespace ScoreTracker.Client.Services
{
    public class AthleteService
    {
        private readonly IAthleteService _athleteClient;

        public AthleteService(IAthleteService athleteClient)
        {
            _athleteClient = athleteClient;
        }

        public Task<Athlete> GetAthleteAsync(string athleteId)
        {
            return _athleteClient.GetAsync(athleteId);
        }

        public IAsyncEnumerable<AthleteResult> GetAthleteResults(AthleteResultQuery query)
        {
            return _athleteClient.SearchAthleteResultsAsync(query);
        }
    }
}