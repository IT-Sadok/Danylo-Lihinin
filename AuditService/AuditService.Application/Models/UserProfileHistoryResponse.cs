namespace AuditService.Application.Models;

public record UserProfileHistoryResponse
{
    public int UserId { get; init; }
    public required CurrentProfile Currrent { get; init; }
    public required List<HistoryItem> Items  { get; init; }
    public int PageCount { get; init; }
    public int PageSize { get; init; }
    public long TotalCount { get; init; }
}