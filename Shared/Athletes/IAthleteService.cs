using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace ScoreTracker.Shared.Athletes
{
    [ServiceContract]
    public interface IAthleteService
    {
        Task<Athlete> GetAsync(string athleteId);
        IAsyncEnumerable<AthleteResult> SearchAthleteResultsAsync(AthleteResultQuery query);
        Task<Athlete> AddAsync(Athlete athlete);
        Task<Athlete> UpdateAsync(Athlete athlete);
    }
}