using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using ScoreTracker.IndexedDb;
using TG.Blazor.IndexedDB;

namespace ScoreTracker.Client.IndexedDb
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIndexedDb(this IServiceCollection services, string name, int version)
        {
            services.AddIndexedDB(dbStore =>
            {
                dbStore.DbName = name;
                dbStore.Version = version;

                var entities =
                    from type in Assembly.GetExecutingAssembly().GetTypes()
                    where type.IsDefined(typeof(IndexedDbStoreAttribute))
                    select type;

                foreach (var entity in entities)
                {
                    IndexSpec primaryKey = null;
                    var indexes = new List<IndexSpec>();
                    foreach (var property in entity.GetProperties())
                    {
                        if (primaryKey == null)
                        {
                            var primaryKeyAttribute = property.GetCustomAttribute<PrimaryKeyAttribute>();
                            if (primaryKeyAttribute != null)
                            {
                                primaryKey = primaryKeyAttribute.ToIndexSpec(property.Name);
                            }
                        }

                        var indexAttribute = property.GetCustomAttribute<IndexAttribute>();
                        if (indexAttribute != null)
                        {
                            indexes.Add(indexAttribute.ToIndexSpec(property.Name));
                        }
                    }

                    dbStore.Stores.Add(new StoreSchema
                    {
                        Name = entity.Name,
                        PrimaryKey = primaryKey,
                        Indexes = indexes,
                    });
                }
            });

            services.AddScoped(typeof(IIndexedDbStoreRepository<,>), typeof(IndexedDbStoreRepository<,>));

            return services;
        }

        public static string ToLowerFirstChar(this string input)
        {
            return char.ToLower(input[0]) + input.Substring(1);
        }
    }
}