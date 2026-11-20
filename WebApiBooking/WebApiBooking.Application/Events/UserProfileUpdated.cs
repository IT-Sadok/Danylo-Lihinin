namespace WebApiBooking.Application.Events;

public record UserProfileUpdated
{
    public required Guid EventId { get; init; }
    public required int SchemaVersion { get; init; }
    public required int UserId { get; init; }
    public int Version { get; init; }
    public  DateTimeOffset OccurredAt { get; init; }
    public required ProfileSnapshot Snapshot { get; init; }
}