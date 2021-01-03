using System.Collections.Generic;
using System.ServiceModel;

namespace ScoreTracker.Shared.Meets
{
    [ServiceContract]
    public interface IMeetClient : IClient<Meet>
    {
        IAsyncEnumerable<Meet> GetAsync(MeetQuery query);
    }
}