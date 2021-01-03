using System.Linq;

namespace ScoreTracker.Shared
{
    public interface IQuery<in TItem, out TResult> where TItem : CosmosEntity
    {
        IQueryable<TResult> ConfigureQuery(IQueryable<TItem> queryable);
    }
}