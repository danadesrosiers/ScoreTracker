using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using ScoreTracker.Shared;

namespace ScoreTracker.Server.Cosmos
{
    public class CosmosRepository<TItem> : ICosmosRepository<TItem> where TItem : class, ICosmosEntity
    {
        private readonly Container _container;

        public CosmosRepository(CosmosCollectionFactory cosmosCollectionFactory)
        {
            _container = cosmosCollectionFactory.GetContainer<TItem>();
        }

        public async Task<TItem> GetItemAsync(string id)
        {
            try
            {
                var response = await _container.ReadItemAsync<TItem>(id, new PartitionKey(id));
                return response.StatusCode == HttpStatusCode.OK ? response.Resource : null;
            }
            catch (CosmosException e) when (e.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public async IAsyncEnumerable<TItem> QueryItemsAsync(Func<IOrderedQueryable<TItem>, IQueryable<TItem>> configureQuery = null)
        {
            // TODO: Force a filter on the partition key.
            configureQuery ??= items => items;
            using var setIterator = configureQuery.Invoke(_container.GetItemLinqQueryable<TItem>()).ToFeedIterator();
            while(setIterator.HasMoreResults)
            {
                foreach (var result in await setIterator.ReadNextAsync())
                {
                    yield return result;
                }
            }
        }

        public async Task AddItemAsync(TItem item)
        {
            await _container.UpsertItemAsync(item, new PartitionKey(item.Id));
        }

        public async Task UpdateItemAsync(TItem item)
        {
            await _container.UpsertItemAsync(item, new PartitionKey(item.Id));
        }

        public async Task DeleteItemAsync(string id)
        {
            await _container.DeleteItemAsync<TItem>(id, new PartitionKey(id));
        }
    }
}