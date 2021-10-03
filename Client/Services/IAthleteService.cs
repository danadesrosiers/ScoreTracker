namespace ScoreTracker.Client.Services;

public interface IAthleteService
{
    Task<Athlete?> GetAthleteAsync(string athleteId);
    IAsyncEnumerable<AthleteResult> GetAthleteResults(AthleteResultQuery query);
}