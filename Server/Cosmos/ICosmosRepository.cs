using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ScoreTracker.Shared;

namespace ScoreTracker.Server.Cosmos
{
    public interface ICosmosRepository<TItem> where TItem : ICosmosEntity
    {
        IAsyncEnumerable<TItem> QueryItemsAsync(Func<IOrderedQueryable<TItem>, IQueryable<TItem>> configureQuery = null);
        Task<TItem> GetItemAsync(string id);
        Task AddItemAsync(TItem item);
        Task UpdateItemAsync(TItem item);
        Task DeleteItemAsync(string id);
    }
}