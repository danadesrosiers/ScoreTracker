namespace ScoreTracker.Shared.Athletes;

[ServiceContract]
public interface IAthleteClient : IClient<Athlete>
{
    IAsyncEnumerable<AthleteResult> GetAsync(AthleteResultQuery query);
}