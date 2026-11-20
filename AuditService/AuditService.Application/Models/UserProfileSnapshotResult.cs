using AuditService.Application.Events;

namespace AuditService.Application.Models;

public record UserProfileSnapshotResult
{
    public int UserId { get; init; }
    public int Version { get; init; }
    public DateTimeOffset OccurredAt { get; init; }
    public required string Name { get; init; }
    public required string Email { get; init; }
}
