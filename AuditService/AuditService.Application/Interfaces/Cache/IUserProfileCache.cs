using AuditService.Application.Models;

namespace AuditService.Application.Interfaces.Cache;

public interface IUserProfileCache
{
    Task<CachedProfile?> GetAsync(int userId, CancellationToken ct);
    Task SetProfileAsync(int userId, UserProfileSnapshotResult profile, CancellationToken ct);
    Task SetEmptyAsync(int userId,CancellationToken ct);
    Task RemoveAsync(int userId, CancellationToken ct);
}