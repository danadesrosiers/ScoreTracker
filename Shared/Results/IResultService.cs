using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace ScoreTracker.Shared.Results
{
    [ServiceContract]
    public interface IResultService
    {
        IAsyncEnumerable<Result> GetAsync(ResultsQuery request);

        Task AddAsync(Result result);
    }
}