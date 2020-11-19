using System;
using System.Collections.Generic;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;
using ScoreTracker.Shared;

namespace ScoreTracker.Server.Cosmos
{
    public class CosmosCollectionFactory
    {
        private readonly Database _database;
        private readonly IServiceCollection _serviceCollection;
        private readonly Dictionary<Type, Container> _containers = new Dictionary<Type, Container>();

        public CosmosCollectionFactory(Database database, IServiceCollection serviceCollection)
        {
            _database = database;
            _serviceCollection = serviceCollection;
        }

        public CosmosCollectionFactory AddCollection<T>() where T : class, ICosmosEntity
        {
            var containerType = typeof(T);
            var containerResponse = _database
                .CreateContainerIfNotExistsAsync(containerType.Name, "/id", 3200)
                .GetAwaiter().GetResult();

            _containers[containerType] = containerResponse.Container;
            _serviceCollection.AddTransient<ICosmosRepository<T>, CosmosRepository<T>>();
            return this;
        }

        public Container GetContainer<TEntity>()
        {
            return _containers[typeof(TEntity)];
        }
    }
}