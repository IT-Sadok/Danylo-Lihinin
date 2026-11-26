namespace AuditService.Application.Models;

public record HistoryItem
{
   public int Version { get; init; }
   public DateTimeOffset OccuredAt { get; init; }
   public string Name { get; init; }
   public string Email { get; init; }
}