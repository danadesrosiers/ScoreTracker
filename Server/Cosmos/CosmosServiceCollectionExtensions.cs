using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ScoreTracker.Shared;

namespace ScoreTracker.Server.Cosmos
{
    public static class CosmosServiceCollectionExtensions
    {
        internal static IServiceCollection AddCosmosClient(
            this IServiceCollection services, IConfiguration configurationSection,
            Action<CosmosCollectionFactory> configureCollections)
        {
            var databaseName = configurationSection.GetSection("DatabaseName").Value;
            var account = configurationSection.GetSection("Account").Value;
            var key = configurationSection.GetSection("Key").Value;

            var clientBuilder = new CosmosClientBuilder(account, key);
            var client = clientBuilder
                .WithConnectionModeDirect()
                .WithSerializerOptions(new CosmosSerializationOptions
                {
                    PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
                })
                .WithThrottlingRetryOptions(TimeSpan.FromSeconds(30), 100)
                .WithBulkExecution(true)
                .Build();

            var database = client.CreateDatabaseIfNotExistsAsync(databaseName).GetAwaiter().GetResult();

            var cosmosCollectionFactory = new CosmosCollectionFactory(database.Database, services);
            configureCollections(cosmosCollectionFactory);

            services.AddSingleton(client.GetDatabase(databaseName));
            services.AddSingleton(client);
            services.AddSingleton(cosmosCollectionFactory);
            return services;
        }
    }
}