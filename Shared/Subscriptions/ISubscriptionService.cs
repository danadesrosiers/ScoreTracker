using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace ScoreTracker.Shared.Subscriptions
{
    [ServiceContract]
    public interface ISubscriptionService
    {
        IAsyncEnumerable<Subscription> GetSubscriptionsAsync(SubscriptionQuery request);

        Task AddSubscriptionAsync(Subscription subscription);
    }
}