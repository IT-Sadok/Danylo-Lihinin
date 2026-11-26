using AuditService.Application.Interfaces.Repositories;
using AuditService.Infrastructure.Messaging;
using AuditService.Infrastructure.Persistence;
using AuditService.Infrastructure.Repositories;
using AuditService.Infrastructure.Settings;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace AuditService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<MongoSettings>()
            .Bind(configuration.GetSection("Mongo"))
            .ValidateDataAnnotations()
            .ValidateOnStart();
        services.AddOptions<RabbitMqSettings>()
            .Bind(configuration.GetSection("RabbitMq"))
            .ValidateDataAnnotations()
            .ValidateOnStart();
        services.AddOptions<RedisSettings>()
            .Bind(configuration.GetSection("Redis"))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddStackExchangeRedisCache(_ => { });
        services.AddOptions<RedisCacheOptions>()
            .Configure<IOptions<RedisSettings>>((cacheOptions, settings) =>
            {
                cacheOptions.Configuration = settings.Value.ConnectionString;
            });
        services.AddHostedService<MongoIndexInitializer>();
        services.AddHostedService<ProfileEventsConsumer>();
        services.AddSingleton<IMongoClient>(sp =>
            new MongoClient(sp.GetRequiredService<IOptions<MongoSettings>>().Value.ConnectionString));
        services.AddSingleton(sp =>
            sp.GetRequiredService<IMongoClient>()
                .GetDatabase(sp.GetRequiredService<IOptions<MongoSettings>>().Value.DatabaseName));
        services.AddSingleton<IUserProfileHistoryRepository, MongoUserProfileHistoryRepository>();
        
        
        return services;
    }
}