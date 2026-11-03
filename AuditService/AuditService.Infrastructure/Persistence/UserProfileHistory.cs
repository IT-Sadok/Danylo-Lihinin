using MongoDB.Bson.Serialization.Attributes;

namespace AuditService.Infrastructure.Persistence;

public class UserProfileHistory
{
    [BsonId]public Guid Id { get; init; }
    public int UserId { get; init; }
    public int Version { get; init; }
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)] public DateTime OccurredAt { get; init; }
    public required ProfileSnapshotDocument Snapshot { get; init; }
}