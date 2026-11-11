using AuditService.Application.Interfaces.Repositories;
using AuditService.Application.Interfaces.Services;
using AuditService.Application.Models;

namespace AuditService.Application.Services;

public class UserProfileAuditService : IUserProfileAuditService
{
    private readonly IUserProfileHistoryRepository _repository;
    
    public UserProfileAuditService(IUserProfileHistoryRepository repository)
    {
        _repository = repository;
    }
    public async Task<UserProfileSnapshotResult> GetUserProfileSnapshotAsync(int userId, DateTimeOffset date, CancellationToken ct)
    {
        var snapshot = await _repository.GetUserProfileSnapshotAsync(userId, date, ct);
        return snapshot;
    }
}