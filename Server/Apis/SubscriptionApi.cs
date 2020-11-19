using System;
using System.Threading.Tasks;
using Grpc.Core;
using ScoreTracker.Proto;
using ScoreTracker.Server.Services.Subscriptions;
using Subscription = ScoreTracker.Proto.Subscription;
using SubscriptionQuery = ScoreTracker.Proto.SubscriptionQuery;

namespace ScoreTracker.Server.Apis
{
    public class SubscriptionApi : Subscriptions.SubscriptionsBase
    {
        private readonly SubscriptionService _subscriptionService;

        public SubscriptionApi(SubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        public override async Task GetSubscriptions(SubscriptionQuery request, IServerStreamWriter<Subscription> responseStream, ServerCallContext context)
        {
            await foreach (var result in _subscriptionService.GetSubscriptionsAsync(request.ToServiceModel()))
            {
                await responseStream.WriteAsync(result.ToSubscriptionApi());
            }
        }

        public override async Task<AddSubscriptionResponse> AddSubscription(Subscription request, ServerCallContext context)
        {
            request.Id = Guid.NewGuid().ToString();
            await _subscriptionService.AddSubscriptionAsync(request.ToServiceModel());

            return new AddSubscriptionResponse();
        }
    }
}