using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using ScoreTracker.Shared;

namespace ScoreTracker.Server.Cosmos
{
    public interface ICosmosRepository<TItem> where TItem : CosmosEntity
    {
        Task<TItem> GetAsync(string id, string partitionKey);
        Task<TItem> GetAsync(string id, double partitionKey);
        Task<TItem> GetAsync(string id, bool partitionKey);
        Task<TItem> GetAsync(string id, PartitionKey? partitionKey = null);
        IAsyncEnumerable<TItem> SearchAsync(Func<IOrderedQueryable<TItem>, IQueryable<TItem>> configureQuery = null);
        Task<TItem> AddAsync(TItem item);
        Task<TItem> UpdateAsync(TItem item);
        Task DeleteItemAsync(string id, string partitionKey = null);
    }
}