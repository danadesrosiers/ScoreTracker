using System.ServiceModel;
using System.Threading.Tasks;

namespace ScoreTracker.Shared
{
    [ServiceContract]
    public interface IClient<TItem> : ISearchableClient<TItem> where TItem : CosmosEntity
    {
        Task<TItem?> GetAsync(Id id);
        Task<TItem> AddAsync(TItem item);
        Task<TItem> UpdateAsync(TItem item);
        Task DeleteAsync(Id id);
    }
}