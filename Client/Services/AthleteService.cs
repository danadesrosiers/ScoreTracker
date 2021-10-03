namespace ScoreTracker.Client.Services;

public class AthleteService : IAthleteService
{
    private readonly IAthleteClient _athleteClient;

    public AthleteService(IAthleteClient athleteClient)
    {
        _athleteClient = athleteClient;
    }

    public Task<Athlete?> GetAthleteAsync(string athleteId)
    {
        return _athleteClient.GetAsync(new Id(athleteId));
    }

    public IAsyncEnumerable<AthleteResult> GetAthleteResults(AthleteResultQuery query)
    {
        return _athleteClient.GetAsync(query);
    }
}