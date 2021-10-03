namespace ScoreTracker.Shared;

public interface ISearchableClient<out TItem> where TItem : CosmosEntity
{
    IAsyncEnumerable<TResult> GetAsync<TResult>(IQuery<TItem, TResult> query);
}