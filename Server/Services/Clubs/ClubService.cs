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

        public async Task<Club> GetClubAsync(string clubId)
        {
            return await _clubRepository.GetItemAsync(clubId);
        }

        public async Task AddClubAsync(Club club)
        {
            await _clubRepository.AddItemAsync(club);
        }
    }
}