using AuditService.Application.Events;
using AuditService.Infrastructure.Persistence;
using AuditService.Infrastructure.Repositories;
using MongoDB.Driver;
using Shouldly;

namespace AuditService.Tests.Infrastructure.Repositories;

public class MongoUserProfileHistoryRepositoryTests : IAsyncLifetime
{
    private static readonly DateTimeOffset T1 = new(2026, 11, 1, 12, 0, 0, TimeSpan.Zero);
    private static readonly DateTimeOffset T2 = new(2026, 11, 5, 10, 0, 0, TimeSpan.Zero);
    private static readonly DateTimeOffset T3 = new(2026, 11, 10, 9, 0, 0, TimeSpan.Zero);

    private readonly IMongoClient _client;
    private readonly string _databaseName;
    private readonly IMongoDatabase _db;
    private readonly MongoUserProfileHistoryRepository _repository;

    public MongoUserProfileHistoryRepositoryTests()
    {
        _client  = new MongoClient("mongodb://localhost:27017");
        _databaseName = $"AuditServiceTests_{Guid.NewGuid():N}";
        _db   = _client.GetDatabase(_databaseName);
        _repository = new MongoUserProfileHistoryRepository(_db);
    }

    public async ValueTask InitializeAsync()
    {
        await new MongoIndexInitializer(_db).StartAsync(TestContext.Current.CancellationToken);
    }

    public async ValueTask DisposeAsync()
    {
        await _client.DropDatabaseAsync(_databaseName);
    }
    private static UserProfileUpdated Event(int userId, int version, DateTimeOffset at, string name) => new()
    {
        EventId = Guid.NewGuid(),
        SchemaVersion = 1,
        UserId = userId,
        Version = version,
        OccurredAt = at,
        Snapshot = new ProfileSnapshot { Name = name, Email = $"{name.ToLower()}@gmail.com" }
    };
    private async Task SeedAsync()
    {
        var ct = TestContext.Current.CancellationToken;
        await _repository.TryAddUserProfileAsync(Event(2, 1, T1, "Danylo"), ct);
        await _repository.TryAddUserProfileAsync(Event(2, 2, T2, "Dima"), ct);
        await _repository.TryAddUserProfileAsync(Event(2, 3, T3, "Oleg"), ct);
    }
    
    [Fact]
    public async Task GetSnapshot_DateBeforeFirstRecord_ReturnsNull()
    {
        var ct = TestContext.Current.CancellationToken;
        await SeedAsync();

        var result = await _repository.GetUserProfileSnapshotAsync(2, T1.AddSeconds(-1), ct);

        result.ShouldBeNull();
    }
    [Theory]
    [InlineData("2026-11-01T12:00:00Z", 1)]   
    [InlineData("2026-11-03T00:00:00Z", 1)]   
    [InlineData("2026-11-07T00:00:00Z", 2)]   
    [InlineData("2026-12-01T00:00:00Z", 3)]   
    public async Task GetSnapshot_DateWithRecords_ReturnsLatestNotAfterDate(string at, int expectedVersion)
    {
        var ct = TestContext.Current.CancellationToken;
        await SeedAsync();

        var result = await _repository.GetUserProfileSnapshotAsync(2, DateTimeOffset.Parse(at), ct);

        result.ShouldNotBeNull();
        result.Version.ShouldBe(expectedVersion);
    }
    [Fact]
    public async Task GetSnapshot_OtherUser_ReturnsNull()
    {
        var ct = TestContext.Current.CancellationToken;
        await SeedAsync();

        var result = await _repository.GetUserProfileSnapshotAsync(99, T3, ct);

        result.ShouldBeNull();
    }
    [Fact]
    public async Task TryAdd_SameEventTwice_SecondCallReturnsFalse()
    {
        var ct = TestContext.Current.CancellationToken;
        var evt = Event(2, 1, T1, "Danylo");

        var first = await _repository.TryAddUserProfileAsync(evt, ct);
        var second = await _repository.TryAddUserProfileAsync(evt, ct);

        first.ShouldBeTrue();
        second.ShouldBeFalse();
    }

}