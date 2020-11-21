using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ScoreTracker.Server.Cosmos;
using ScoreTracker.Shared.Results;
using static System.String;

namespace ScoreTracker.Server.Services.Results
{
    public class MeetResultService : IResultService
    {
        private readonly ICosmosRepository<Result> _resultRepository;

        public MeetResultService(ICosmosRepository<Result> resultRepository)
        {
            _resultRepository = resultRepository;
        }

        public IAsyncEnumerable<Result> GetResultsAsync(ResultsQuery request)
        {
            return _resultRepository.QueryItemsAsync(request.ConfigureQuery);
        }

        public async Task AddResultAsync(Result result)
        {
            await _resultRepository.AddItemAsync(result);
        }
    }
}