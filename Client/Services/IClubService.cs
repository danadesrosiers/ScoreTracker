using ScoreTracker.Shared.Clubs;

namespace ScoreTracker.Client.Services;

public interface IClubService
{
    Task<Club?> GetClubAsync(string clubId);
}