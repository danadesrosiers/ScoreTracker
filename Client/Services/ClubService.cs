using System.Threading.Tasks;
using ScoreTracker.Shared.Clubs;

namespace ScoreTracker.Client.Services
{
    public class ClubService
    {
        private readonly IClubService _clubClient;

        public ClubService(IClubService clubClient)
        {
            _clubClient = clubClient;
        }

        public Task<Club?> GetClubAsync(string clubId)
        {
            return _clubClient.GetAsync(clubId);
        }
    }
}