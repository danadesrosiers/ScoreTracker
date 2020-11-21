using System.Collections.Generic;
using System.Threading.Tasks;
using ScoreTracker.Server.Cosmos;
using ScoreTracker.Shared.Subscriptions;

namespace ScoreTracker.Server.Services.Subscriptions
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly ICosmosRepository<Subscription> _subscriptionRepository;

        public SubscriptionService(ICosmosRepository<Subscription> subscriptionRepository)
        {
            _subscriptionRepository = subscriptionRepository;
        }

        public IAsyncEnumerable<Subscription> GetSubscriptionsAsync(SubscriptionQuery request)
        {
            return _subscriptionRepository.QueryItemsAsync();
        }

        public async Task AddSubscriptionAsync(Subscription subscription)
        {
            await _subscriptionRepository.AddItemAsync(subscription);
        }
    }
}