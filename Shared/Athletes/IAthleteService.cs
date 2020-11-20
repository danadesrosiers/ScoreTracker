using System.ServiceModel;
using System.Threading.Tasks;

namespace ScoreTracker.Shared.Athletes
{
    [ServiceContract]
    public interface IAthleteService
    {
        Task<Athlete> GetAthleteAsync(string athleteId);

        Task AddAthleteAsync(Athlete athlete);
    }
}