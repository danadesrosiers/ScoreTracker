using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ScoreTracker.Server.Cosmos;
using ScoreTracker.Shared;
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

        public async Task<Meet> GetMeetAsync(string meetId)
        {
            return await _meetRepository.GetItemAsync(meetId);
        }

        public IAsyncEnumerable<Meet> GetMeetsAsync(MeetQuery meetQuery)
        {
            return _meetRepository.QueryItemsAsync(meetQuery.ConfigureQuery);
        }

        public async Task AddMeetAsync(Meet meet)
        {
            await _meetRepository.AddItemAsync(meet);
        }
    }
}