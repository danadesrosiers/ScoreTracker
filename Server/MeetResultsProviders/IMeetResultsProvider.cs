using ScoreTracker.Server.MeetResultsProviders.MyUsaGym;
using ScoreTracker.Shared.Meets;

namespace ScoreTracker.Server.MeetResultsProviders;

public interface IMeetResultsProvider
{
    Task<MeetInfo?> GetMeetInfoAsync(string meetId);
    Task<Meet?> GetMeetAsync(string meetId);
    Task<List<Meet>> SearchMeetsAsync(MeetQuery query);
}