using System.Threading.Tasks;
using ScoreTracker.Server.Cosmos;
using ScoreTracker.Shared.Clubs;

namespace ScoreTracker.Server.Services.Clubs
{
    public class ClubService : IClubService
    {
        private readonly ICosmosRepository<Club> _clubRepository;

        public ClubService(ICosmosRepository<Club> clubRepository)
        {
            _clubRepository = clubRepository;
        }

        public async Task<Club?> GetAsync(string clubId)
        {
            return await _clubRepository.GetAsync(clubId);
        }

        public async Task<Club> AddAsync(Club club)
        {
            return await _clubRepository.AddAsync(club);
        }

        public async Task<Club> UpdateAsync(Club club)
        {
            return await _clubRepository.UpdateAsync(club);
        }
    }
}