namespace ScoreTracker.Client.Services;

public interface IUserService
{
    event Action? OnUserChange;
    User? User { get; set; }
    Task LogInAsync(string identityReference);
    Task LogOut();
    Task<User?> GetUserAsync();
    Task UpdateUserAsync(User user);
    Task FollowAthleteAsync(string athleteId, string name);
    Task FollowClubAsync(string clubId, string name);
    bool IsLoggedIn();
    Task StopFollowingAthleteAsync(string athleteId);
    Task StopFollowingClubAsync(string clubId);
}