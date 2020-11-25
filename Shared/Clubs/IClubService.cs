using System.ServiceModel;
using System.Threading.Tasks;

namespace ScoreTracker.Shared.Clubs
{
    [ServiceContract]
    public interface IClubService
    {
        Task<Club> GetAsync(string clubId);

        Task AddAsync(Club club);
    }
}