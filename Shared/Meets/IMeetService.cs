using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace ScoreTracker.Shared.Meets
{
    [ServiceContract]
    public interface IMeetService
    {
        Task<Meet> GetAsync(string meetId);

        IAsyncEnumerable<Meet> SearchAsync(MeetQuery meetQuery);

        Task<Meet> AddAsync(Meet meet);
    }
}