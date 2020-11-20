using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace ScoreTracker.Shared.Results
{
    [ServiceContract]
    public interface IResultService
    {
        IAsyncEnumerable<Result> GetResultsAsync(ResultsQuery request);

        Task AddResultAsync(Result result);
    }
}