using System.Collections.Generic;
using System.Threading.Tasks;
using ScoreTracker.Server.Services.Meets;
using ScoreTracker.Shared.Meets;

namespace ScoreTracker.Server.Services.Results
{
    public interface IMeetResultsProvider
    {
        Task<MeetInfo> GetMeetInfoAsync(string meetId);
        Task<Meet> GetMeetAsync(string meetId);
        Task<List<Meet>> SearchMeetsAsync(MeetQuery query);
    }
}