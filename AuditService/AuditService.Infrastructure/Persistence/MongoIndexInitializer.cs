using Microsoft.Extensions.Hosting;
using MongoDB.Driver;

namespace AuditService.Infrastructure.Persistence;

public class MongoIndexInitializer : IHostedService
{
    private readonly IMongoDatabase _database;
    
    public MongoIndexInitializer(IMongoDatabase database) => _database = database;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var collection =  _database.GetCollection<UserProfileHistory>(MongoCollections.UserProfileHistory);
        
        var keys = Builders<UserProfileHistory>.IndexKeys
            .Ascending(x => x.UserId)
            .Descending(x => x.OccurredAt);
        
        await collection.Indexes.CreateOneAsync(new CreateIndexModel<UserProfileHistory>(keys), cancellationToken: cancellationToken);
    }
    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}