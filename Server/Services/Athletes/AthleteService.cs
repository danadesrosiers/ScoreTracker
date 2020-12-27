using System.Collections.Generic;
using System.Threading.Tasks;
using ScoreTracker.Server.Cosmos;
using ScoreTracker.Shared.Athletes;

namespace ScoreTracker.Server.Services.Athletes
{
    public class AthleteService : IAthleteService
    {
        private readonly ICosmosRepository<Athlete> _athleteRepository;

        public AthleteService(ICosmosRepository<Athlete> athleteRepository)
        {
            _athleteRepository = athleteRepository;
        }

        public async Task<Athlete?> GetAsync(string athleteId)
        {
            return await _athleteRepository.GetAsync(athleteId);
        }

        public IAsyncEnumerable<AthleteResult> SearchAthleteResultsAsync(AthleteResultQuery query)
        {
            return _athleteRepository.SearchAsync(query.ConfigureQuery);
        }

        public async Task<Athlete> AddAsync(Athlete athlete)
        {
            return await _athleteRepository.AddAsync(athlete);
        }

        public async Task<Athlete> UpdateAsync(Athlete athlete)
        {
            return await _athleteRepository.UpdateAsync(athlete);
        }
    }
}