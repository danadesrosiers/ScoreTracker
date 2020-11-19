using System.Collections.Generic;
using System.Threading.Tasks;
using ScoreTracker.Shared;

namespace ScoreTracker.Server.Cosmos
{
    public interface ICosmosRepository<TItem> where TItem : ICosmosEntity
    {
        Task<IEnumerable<TItem>> GetItemsAsync(string query = "1=1", int? limit = null);
        Task<TItem> GetItemAsync(string id);
        Task AddItemAsync(TItem item);
        Task UpdateItemAsync(TItem item);
        Task DeleteItemAsync(string id);
    }
}