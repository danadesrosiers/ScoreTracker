using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace ScoreTracker.Shared.Results
{
    [ServiceContract]
    public interface IResultService
    {
        IAsyncEnumerable<MeetResult> GetAsync(ResultsQuery request);

        Task AddAsync(MeetResult meetResult);
    }
}