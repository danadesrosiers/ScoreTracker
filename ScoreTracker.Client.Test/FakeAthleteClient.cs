namespace ScoreTracker.Client.Test;

public class FakeAthleteClient : IAthleteClient
{
    private readonly Dictionary<Id, Athlete> _athletes = new();

    public IAsyncEnumerable<TResult> GetAsync<TResult>(IQuery<Athlete, TResult> query)
    {
        return new List<TResult>().ToAsyncEnumerable();
    }

    public Task<Athlete?> GetAsync(Id id)
    {
        return Task.FromResult<Athlete?>(_athletes[id]);
    }

    public Task<Athlete> AddAsync(Athlete athlete)
    {
        return AddOrUpdateAsync(athlete);
    }

    public Task<Athlete> UpdateAsync(Athlete athlete)
    {
        return AddOrUpdateAsync(athlete);
    }

    public Task<Athlete> AddOrUpdateAsync(Athlete athlete)
    {
        _athletes[new Id(athlete.Id!)] = athlete;
        return Task.FromResult(athlete);
    }

    public Task DeleteAsync(Id id)
    {
        _athletes.Remove(id);
        return Task.CompletedTask;
    }

    public IAsyncEnumerable<AthleteResult> GetAsync(AthleteResultQuery query)
    {
        return query.ConfigureQuery(_athletes.Values.AsQueryable()).ToAsyncEnumerable();
    }
}