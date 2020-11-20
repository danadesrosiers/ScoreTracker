using System.ServiceModel;
using System.Threading.Tasks;

namespace ScoreTracker.Shared.Clubs
{
    [ServiceContract]
    public interface IClubService
    {
        Task<Club> GetClubAsync(string clubId);

        Task AddClubAsync(Club club);
    }
}