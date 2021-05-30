using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using ScoreTracker.Shared;

namespace ScoreTracker.Server.Cosmos
{
    public class CosmosClient<TItem> : IClient<TItem> where TItem : CosmosEntity
    {
        private readonly Container _container;

        public CosmosClient(CosmosCollectionFactory cosmosCollectionFactory)
        {
            _container = cosmosCollectionFactory.GetContainer<TItem>();
        }

        public Task<TItem?> GetAsync(Id id) => GetAsync(id.Value, null);

        private async Task<TItem?> GetAsync(string id, PartitionKey? partitionKey)
        {
            try
            {
                var response = await _container.ReadItemAsync<TItem>(id, partitionKey ?? new PartitionKey(id));
                return response.StatusCode == HttpStatusCode.OK ? response.Resource : null;
            }
            catch (CosmosException e) when (e.StatusCode == HttpStatusCode.NotFound)
            {
                return default;
            }
        }

        public async IAsyncEnumerable<TResult> GetAsync<TResult>(IQuery<TItem, TResult> query)
        {
            using var setIterator = query.ConfigureQuery(_container.GetItemLinqQueryable<TItem>()).ToFeedIterator();
            while(setIterator.HasMoreResults)
            {
                foreach (var result in await setIterator.ReadNextAsync())
                {
                    yield return result;
                }
            }
        }

        public async Task<TItem> AddAsync(TItem item)
        {
            return (await _container.CreateItemAsync(item)).Resource;
        }

        public async Task<TItem> UpdateAsync(TItem item)
        {
            if (string.IsNullOrEmpty(item.ETag))
            {
                throw new InvalidOperationException($"Etag is missing on {item.GetType().Name}.");
            }
            var options = new ItemRequestOptions { IfMatchEtag = item.ETag };
            return (await _container.UpsertItemAsync(item, null, options)).Resource;
        }

        public async Task<TItem> AddOrUpdateAsync(TItem item)
        {
            var options = new ItemRequestOptions { IfMatchEtag = item.ETag };
            return (await _container.UpsertItemAsync(item, null, options)).Resource;
        }

        public async Task DeleteAsync(Id id)
        {
            await _container.DeleteItemAsync<TItem>(id.Value, new PartitionKey(id.Value));
        }
    }
}