namespace AuditService.Infrastructure.Persistence;

public class ProfileSnapshotDocument
{
    public required string Name { get; init; }
    public required string Email { get; init; }
}