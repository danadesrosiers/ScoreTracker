using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace ScoreTracker.Shared.Meets
{
    [ServiceContract]
    public interface IMeetService
    {
        Task<Meet> GetMeetAsync(string meetId);

        IAsyncEnumerable<Meet> GetMeetsAsync(MeetQuery query);

        Task AddMeetAsync(Meet meet);
    }
}