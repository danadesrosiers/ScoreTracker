using System.Collections.Generic;
using System.Threading.Tasks;
using ScoreTracker.Shared;
using ScoreTracker.Shared.Meets;
using ScoreTracker.Shared.Results;
using ScoreTracker.Shared.Users;

namespace ScoreTracker.Client.Services
{
    public interface IMeetService
    {
        Task<Meet?> GetMeetAsync(string meetId);
        Task<Dictionary<string, string>> SearchMeetsAsync(int selectedSeason, StateCode? selectedState, Discipline? selectedDiscipline, string? searchString);
        Task<ICollection<Meet>> GetFollowingMeetsAsync(User user);
        Task<ICollection<MeetResult>> GetResults(string meetId, IEnumerable<string> divisions);
        ICollection<MeetResult> CalculateTeamResults(IEnumerable<MeetResult> results);
    }
}