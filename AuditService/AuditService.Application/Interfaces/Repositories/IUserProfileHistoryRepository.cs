using AuditService.Application.Events;
using AuditService.Application.Models;

namespace AuditService.Application.Interfaces.Repositories;

public interface IUserProfileHistoryRepository
{
    public Task<bool> TryAddUserProfileAsync(UserProfileUpdated updateEvent, CancellationToken ct);
    public Task<UserProfileSnapshotResult?> GetUserProfileSnapshotAsync(int userId, DateTimeOffset at, CancellationToken ct);
}