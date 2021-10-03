namespace ScoreTracker.Client.Services;

public interface IMeetService
{
    Task<Meet?> GetMeetAsync(string meetId);
    Task<Dictionary<string, string>> SearchMeetsAsync(int selectedSeason, StateCode? selectedState, Discipline? selectedDiscipline, string? searchString);
    Task<ICollection<Meet>> GetFollowingMeetsAsync(User user);
    Task<ICollection<MeetResult>> GetResults(string meetId, IEnumerable<string> divisions);
    ICollection<MeetResult> CalculateTeamResults(IEnumerable<MeetResult> results);
}