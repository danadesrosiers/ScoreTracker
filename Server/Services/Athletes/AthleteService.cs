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

        public async Task<Athlete> GetAthleteAsync(string athleteId)
        {
            return await _athleteRepository.GetItemAsync(athleteId);
        }

        public async Task AddAthleteAsync(Athlete athlete)
        {
            await _athleteRepository.AddItemAsync(athlete);
        }
    }
}