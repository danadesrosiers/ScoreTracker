namespace ScoreTracker.Client.Test;

public class FakeClubClient : IClubClient
{
    private readonly Dictionary<Id, Club> _clubs = new();

    public IAsyncEnumerable<TResult> GetAsync<TResult>(IQuery<Club, TResult> query)
    {
        return new List<TResult>().ToAsyncEnumerable();
    }

    public Task<Club?> GetAsync(Id id)
    {
        return Task.FromResult<Club?>(_clubs[id]);
    }

    public Task<Club> AddAsync(Club club)
    {
        return AddOrUpdateAsync(club);
    }

    public Task<Club> UpdateAsync(Club club)
    {
        return AddOrUpdateAsync(club);
    }

    public Task<Club> AddOrUpdateAsync(Club club)
    {
        _clubs[new Id(club.Id!)] = club;
        return Task.FromResult(club);
    }

    public Task DeleteAsync(Id id)
    {
        _clubs.Remove(id);
        return Task.CompletedTask;
    }
}