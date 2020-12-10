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
        private readonly ICosmosRepository<MeetResult> _resultRepository;

        public MeetResultService(ICosmosRepository<MeetResult> resultRepository)
        {
            _resultRepository = resultRepository;
        }

        public IAsyncEnumerable<MeetResult> GetAsync(ResultsQuery request)
        {
            return _resultRepository.SearchAsync(request.ConfigureQuery);
        }

        public async Task AddAsync(MeetResult meetResult)
        {
            await _resultRepository.AddAsync(meetResult);
        }
    }
}