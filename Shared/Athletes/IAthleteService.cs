using System.ServiceModel;
using System.Threading.Tasks;

namespace ScoreTracker.Shared.Athletes
{
    [ServiceContract]
    public interface IAthleteService
    {
        Task<Athlete> GetAsync(string athleteId);

        Task<Athlete> AddAsync(Athlete athlete);
        Task<Athlete> UpdateAsync(Athlete athlete);
    }
}