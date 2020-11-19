using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
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

        public async Task<IEnumerable<TItem>> GetItemsAsync(string filterValue = "1=1", int? limit = null)
        {
            // TODO: Force a filter on the partition key.
            var limitSql = limit != null ? "TOP " + limit : "";
            var queryString = $"select {limitSql} * from c WHERE {filterValue}";
            var query = _container.GetItemQueryIterator<TItem>(new QueryDefinition(queryString));
            var results = new List<TItem>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();

                results.AddRange(response.ToList());
            }

            return results;
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