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

        public async IAsyncEnumerable<Meet> GetMeetsAsync(MeetQuery query)
        {
            var meets = await _meetRepository.GetItemsAsync();
            var filteredMeets = meets.Where(m =>
                (query.StateCode == StateCode.Any || m.State == query.StateCode) &&
                (query.Discipline == Discipline.All || m.Discipline == query.Discipline) &&
                (query.Year == 0 || m.Season == query.Year));

            foreach (var meet in filteredMeets)
            {
                yield return meet;
            }
        }

        public async Task AddMeetAsync(Meet meet)
        {
            await _meetRepository.AddItemAsync(meet);
        }
    }
}