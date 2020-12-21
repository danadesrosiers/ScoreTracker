using System.Collections.Generic;
using System.Threading.Tasks;
using ScoreTracker.Server.Cosmos;
using ScoreTracker.Shared.Meets;

namespace ScoreTracker.Server.Services.Meets
{
    public class MeetService : IMeetService
    {
        private readonly ICosmosRepository<Meet> _meetRepository;

        public MeetService(ICosmosRepository<Meet> meetRepository)
        {
            _meetRepository = meetRepository;
        }

        public async Task<Meet> GetAsync(string meetId)
        {
            return await _meetRepository.GetAsync(meetId);
        }

        public IAsyncEnumerable<Meet> SearchAsync(MeetQuery meetQuery)
        {
            return _meetRepository.SearchAsync(meetQuery.ConfigureQuery);
        }

        public async Task<Meet> AddAsync(Meet meet)
        {
            return await _meetRepository.AddAsync(meet);
        }
    }
}