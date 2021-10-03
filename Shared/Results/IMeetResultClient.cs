namespace ScoreTracker.Shared.Results;

[ServiceContract]
public interface IMeetResultClient : IClient<MeetResult>
{
    IAsyncEnumerable<MeetResult> GetAsync(ResultsQuery query);
}