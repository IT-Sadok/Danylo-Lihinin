namespace AuditService.Application.Events;

public record UserProfileUpdated
{
    public Guid EventId { get; init; }
    public int SchemaVersion { get; init; }
    public int UserId { get; init; }
    public int Version { get; init; }
    public DateTimeOffset OccurredAt { get; init; }
    public ProfileSnapshot Snapshot { get; init; } 
}