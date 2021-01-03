using System.Collections.Generic;
using System.ServiceModel;

namespace ScoreTracker.Shared.Results
{
    [ServiceContract]
    public interface IMeetResultClient : IClient<MeetResult>
    {
        IAsyncEnumerable<MeetResult> GetAsync(ResultsQuery query);
    }
}