using System.Collections.Generic;
using System.Threading.Tasks;
using ScoreTracker.Server.Cosmos;
using ScoreTracker.Shared.Results;

namespace ScoreTracker.Server.Services.Results
{
    public class MeetResultService : IResultService
    {
        private readonly ICosmosRepository<MeetResult> _resultRepository;

        public MeetResultService(ICosmosRepository<MeetResult> resultRepository)
        {
            _resultRepository = resultRepository;
        }

        public IAsyncEnumerable<MeetResult> GetAsync(ResultsQuery request)
        {
            return _resultRepository.SearchAsync(request.ConfigureQuery);
        }

        public async Task<MeetResult> AddAsync(MeetResult meetResult)
        {
            return await _resultRepository.AddAsync(meetResult);
        }
    }
}