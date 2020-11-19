using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ScoreTracker.Server.Cosmos;

namespace ScoreTracker.Server.Services.Meets
{
    public class MeetService
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

        public async Task<IEnumerable<Meet>> GetMeetsAsync(MeetQuery query)
        {
            var meets = await _meetRepository.GetItemsAsync();
            return meets.Where(m =>
                (query.StateCode == StateCode.Any || m.State == query.StateCode) &&
                (query.Discipline == Discipline.All || m.Discipline == query.Discipline) &&
                (query.Year == 0 || m.Season == query.Year));
        }

        public async Task AddMeetAsync(Meet meet)
        {
            await _meetRepository.AddItemAsync(meet);
        }
    }
}