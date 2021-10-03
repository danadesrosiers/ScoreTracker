namespace ScoreTracker.Server;

public static class EnumerableExtensions
{
    public static async Task<List<TOutput>> SelectManyAsync<TInput, TOutput>(
        this IEnumerable<TInput> inputs,
        Func<TInput, Task<IEnumerable<TOutput>>> func)
    {
        // Initiate all tasks in parallel.
        var tasks = inputs.Select(func);

        // Collect the results.
        var results = new List<TOutput>();
        foreach (var task in tasks)
        {
            results.AddRange(await task);
        }

        return results;
    }
}