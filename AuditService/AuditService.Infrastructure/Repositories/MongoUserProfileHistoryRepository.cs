using AuditService.Application.Events;
using AuditService.Application.Interfaces.Repositories;
using AuditService.Application.Models;
using AuditService.Infrastructure.Persistence;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AuditService.Infrastructure.Repositories;

public class MongoUserProfileHistoryRepository : IUserProfileHistoryRepository
{
    private readonly IMongoCollection<UserProfileHistory> _collection;

    public MongoUserProfileHistoryRepository(IMongoDatabase database)
    {
        _collection = database.GetCollection<UserProfileHistory>(MongoCollections.UserProfileHistory);
    }

    public async Task<bool> TryAddUserProfileAsync(UserProfileUpdated updateEvent, CancellationToken ct)
    {
        var userHistory = new UserProfileHistory
        {
            Id = updateEvent.EventId,
            OccurredAt = updateEvent.OccurredAt.UtcDateTime,
            UserId = updateEvent.UserId,
            Version = updateEvent.Version,
            Snapshot = new ProfileSnapshotDocument
            {
                Email = updateEvent.Snapshot.Email,
                Name = updateEvent.Snapshot.Name
            }
        };
        try
        { 
            await _collection.InsertOneAsync(userHistory, cancellationToken: ct);
            return true;
        }
        catch(MongoWriteException ex) when (ex.WriteError.Category == ServerErrorCategory.DuplicateKey)
        {
            return false;
        }
        
    }

    public async Task<UserProfileSnapshotResult?> GetUserProfileSnapshotAsync(int userId,DateTimeOffset at, CancellationToken ct)
    {
        var doc = await _collection.Find(x => x.UserId == userId && x.OccurredAt <= at.UtcDateTime)
            .SortByDescending(x => x.OccurredAt)
            .ThenByDescending(x => x.Version)
            .FirstOrDefaultAsync(ct);

        if (doc is null)
            return null;
        var userProfile = new UserProfileSnapshotResult
        {
            UserId = userId,
            Version = doc.Version,
            OccurredAt =  new DateTimeOffset(DateTime.SpecifyKind(doc.OccurredAt, DateTimeKind.Utc)),
            Name = doc.Snapshot.Name,
            Email = doc.Snapshot.Email
        };
        return userProfile;
    }
}