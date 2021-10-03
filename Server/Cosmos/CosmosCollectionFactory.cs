using Microsoft.Azure.Cosmos;

namespace ScoreTracker.Server.Cosmos;

public class CosmosCollectionFactory
{
    private readonly Database _database;
    private readonly IServiceCollection _serviceCollection;
    private readonly Dictionary<Type, Container> _containers = new();

    public CosmosCollectionFactory(Database database, IServiceCollection serviceCollection)
    {
        _database = database;
        _serviceCollection = serviceCollection;
    }

    public CosmosCollectionFactory AddCollection<T>(string partitionKeyPath = "/id") where T : CosmosEntity
    {
        var containerType = typeof(T);
        var containerResponse = _database
            .CreateContainerIfNotExistsAsync(containerType.Name, partitionKeyPath)
            .GetAwaiter().GetResult();

        _containers[containerType] = containerResponse.Container;
        _serviceCollection.AddTransient<IClient<T>, CosmosClient<T>>();
        return this;
    }

    public Container GetContainer<TEntity>()
    {
        return _containers[typeof(TEntity)];
    }

    public async Task<Container> GetLeaseContainer()
    {
        return await _database.CreateContainerIfNotExistsAsync("lease", "/id");
    }
}