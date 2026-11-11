using AuditService.Application.Models;

namespace AuditService.Application.Interfaces.Services;

public interface IUserProfileAuditService
{
    public Task<UserProfileSnapshotResult> GetUserProfileSnapshotAsync(int userId, DateTimeOffset date, CancellationToken ct);
}