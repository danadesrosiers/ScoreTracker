using System.Collections.Generic;
using System.Threading.Tasks;
using TG.Blazor.IndexedDB;

namespace ScoreTracker.IndexedDb
{
    public class IndexedDbStoreRepository<TEntity, TIndexEnum> : IIndexedDbStoreRepository<TEntity, TIndexEnum>
    {
        private readonly IndexedDBManager _dbManager;

        public IndexedDbStoreRepository(IndexedDBManager dbManager)
        {
            _dbManager = dbManager;
        }

        public async Task<TEntity> GetById(int id)
        {
            var entity = await _dbManager.GetRecordById<int, TEntity>(typeof(TEntity).Name, id);
            return entity;
        }

        public async Task<IList<TEntity>> GetAll()
        {
            var name = typeof(TEntity).Name;
            return await _dbManager.GetRecords<TEntity>(typeof(TEntity).Name) ?? new List<TEntity>();
        }

        public async Task<IList<TEntity>> GetAll<T>(TIndexEnum indexName, T queryValue)
        {
            return await _dbManager.GetAllRecordsByIndex<T, TEntity>(new StoreIndexQuery<T>
            {
                Storename = typeof(TEntity).Name,
                IndexName = indexName.ToString(),
                QueryValue = queryValue,
            }) ?? new List<TEntity>();
        }

        public async Task Add(TEntity entity)
        {
            await _dbManager.AddRecord(new StoreRecord<TEntity>
            {
                Storename = typeof(TEntity).Name,
                Data = entity,
            });
        }

        public async Task Update(TEntity entity)
        {
            await _dbManager.UpdateRecord(new StoreRecord<TEntity>
            {
                Storename = typeof(TEntity).Name,
                Data = entity,
            });
        }

        public async Task Delete(int id)
        {
            await _dbManager.DeleteRecord(typeof(TEntity).Name, id);
        }
    }
}