using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Rtl.Configuration.Validation;
using TvMazeScrapperDp.Core.Mappings;
using TvMazeScrapperDP.Persistance.MongoDb.Mapping;
using TvMazeScrapperDP.Persistance.MongoDb.Model;
using TvMazeScrapperDP.Persistance.MongoDb.Services;
using TvMazeScrapperDP.Persistance.MongoDb.Services.Contracts;

namespace TvMazeScrapperDP.Persistance.MongoDb
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services,
            IConfiguration configuration)
        {
            return services
                .AddConfig<MongoDbConfig>(configuration, "MongoDb")
                .AddSingleton<IMongoDbClientFactory, MongoDbClientFactory>()
                .AddSingleton<IMongoDbClient>(provider => provider
                    .GetRequiredService<IMongoDbClientFactory>()
                    .Create(provider.GetRequiredService<IOptions<MongoDbConfig>>().Value.ConnectionString))
                .AddSingleton<IMongoDatabase>(provider => provider
                    .GetRequiredService<IMongoDbClient>()
                    .GetDatabase(provider.GetRequiredService<IOptions<MongoDbConfig>>().Value.Database));
        }

        public static IServiceCollection AddMappings(this IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new CastMappingProfile());
                mc.AddProfile(new ShowMappingProfile());
            });

            var mapper = mappingConfig.CreateMapper();
            return services.AddSingleton(mapper);
        }
    }
}