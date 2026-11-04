using AuditService.Application.Interfaces.Repositories;
using AuditService.Infrastructure.Persistence;
using AuditService.Infrastructure.Persistence.Settings;
using AuditService.Infrastructure.Repositories;
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
        
        services.AddHostedService<MongoIndexInitializer>();
        services.AddSingleton<IMongoClient>(sp =>
            new MongoClient(sp.GetRequiredService<IOptions<MongoSettings>>().Value.ConnectionString));
        services.AddSingleton(sp =>
            sp.GetRequiredService<IMongoClient>()
                .GetDatabase(sp.GetRequiredService<IOptions<MongoSettings>>().Value.DatabaseName));
        services.AddSingleton<IUserProfileHistoryRepository, MongoUserProfileHistoryRepository>();
        
        services.AddHostedService<MongoIndexInitializer>();
        
        return services;
    }
}