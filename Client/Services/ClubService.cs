using ScoreTracker.Shared;
using ScoreTracker.Shared.Clubs;

namespace ScoreTracker.Client.Services;

public class ClubService : IClubService
{
    private readonly IClubClient _clubClient;

    public ClubService(IClubClient clubClient)
    {
        _clubClient = clubClient;
    }

    public Task<Club?> GetClubAsync(string clubId)
    {
        return _clubClient.GetAsync(new Id(clubId));
    }
}