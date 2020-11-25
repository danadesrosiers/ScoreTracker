using System;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ScoreTracker.Server.Cosmos
{
    public static class CosmosServiceCollectionExtensions
    {
        private const int MaxRetryWaitTimeOnThrottledRequests = 30; // In seconds.
        private const int MaxRetryAttemptsOnThrottledRequests = 100;
        private const int MaxAutoScaleThroughput = 4000;

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
                .WithThrottlingRetryOptions(TimeSpan.FromSeconds(MaxRetryWaitTimeOnThrottledRequests), MaxRetryAttemptsOnThrottledRequests)
                .WithBulkExecution(true)
                .Build();

            var throughput = ThroughputProperties.CreateAutoscaleThroughput(MaxAutoScaleThroughput);
            var database = client.CreateDatabaseIfNotExistsAsync(databaseName, throughput).GetAwaiter().GetResult();

            var cosmosCollectionFactory = new CosmosCollectionFactory(database.Database, services);
            configureCollections(cosmosCollectionFactory);

            services.AddSingleton(client.GetDatabase(databaseName));
            services.AddSingleton(client);
            services.AddSingleton(cosmosCollectionFactory);
            return services;
        }
    }
}