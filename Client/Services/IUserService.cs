using System;
using System.Threading.Tasks;
using ScoreTracker.Shared.Users;

namespace ScoreTracker.Client.Services
{
    public interface IUserService
    {
        event Action? OnUserChange;
        User? User { get; set; }
        Task LogIn(string identityReference);
        Task LogOut();
        Task<User?> GetUserAsync();
        Task UpdateUserAsync(User user);
        Task FollowAthleteAsync(string athleteId, string name);
        Task FollowClubAsync(string clubId, string name);
        Task FollowAsync(Subscription subscription);
        bool IsFollowingClub(string clubId);
        bool IsFollowingAthlete(string athleteId);
        bool IsLoggedIn();
    }
}