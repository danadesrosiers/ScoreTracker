using System.Collections.Generic;
using System.Threading.Tasks;

namespace ScoreTracker.IndexedDb
{
    public interface IIndexedDbStoreRepository<TEntity, TIndexEnum>
    {
        Task<TEntity> GetById(int id);
        Task<IList<TEntity>> GetAll();
        Task<IList<TEntity>> GetAll<T>(TIndexEnum indexName, T queryValue);
        Task Add(TEntity entity);
        Task Update(TEntity entity);
        Task Delete(int id);
    }
}