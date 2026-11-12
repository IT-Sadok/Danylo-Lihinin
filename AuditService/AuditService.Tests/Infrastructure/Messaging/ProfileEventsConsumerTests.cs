using System.Text;
using System.Text.Json;
using AuditService.Application.Events;
using AuditService.Application.Interfaces.Repositories;
using AuditService.Infrastructure.Messaging;
using AuditService.Infrastructure.Settings;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace AuditService.Tests.Infrastructure.Messaging;

public class ProfileEventsConsumerTests
{
    private const ulong Tag = 1;
    
    private readonly Mock<IUserProfileHistoryRepository> _repository = new();
    private readonly Mock<IChannel> _channel = new();
    private readonly ProfileEventsConsumer _consumer;

    public ProfileEventsConsumerTests()
    {
        _consumer = new ProfileEventsConsumer(                                          
            _repository.Object,
            NullLogger<ProfileEventsConsumer>.Instance,
            Options.Create(new RabbitMqSettings
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest",
                QueueName = "audit.user-profile"
            }));
    }
    private static UserProfileUpdated Valid() => new()
    {
        EventId = Guid.NewGuid(),
        SchemaVersion = 1,
        UserId = 2,
        Version = 1,
        OccurredAt = new DateTimeOffset(2026, 11, 1, 12, 0, 0, TimeSpan.Zero),
        Snapshot = new ProfileSnapshot { Name = "Danylo", Email = "danylo@gmail.com" }
    };
    private static BasicDeliverEventArgs Delivery(string body) =>
        new(
            consumerTag: "test",
            deliveryTag: Tag,
            redelivered: false,
            exchange: string.Empty,
            routingKey: "audit.user-profile",
            properties: new BasicProperties { MessageId = Guid.NewGuid().ToString() },
            body: Encoding.UTF8.GetBytes(body));
    private static string Json(UserProfileUpdated e) => JsonSerializer.Serialize(e);

    private void VerifyAcked(Times times) =>
        _channel.Verify(c => c.BasicAckAsync(Tag, false, It.IsAny<CancellationToken>()), times);

    private void VerifyNacked(bool requeue, Times times) =>
        _channel.Verify(c => c.BasicNackAsync(Tag, false, requeue, It.IsAny<CancellationToken>()), times);

    private void SetupRepository(bool result) =>
        _repository
            .Setup(r => r.TryAddUserProfileAsync(It.IsAny<UserProfileUpdated>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);
    [Fact]
    public async Task Handle_NewEvent_AcksMessage()
    {
        var ct = TestContext.Current.CancellationToken;
        SetupRepository(true);

        await _consumer.HandleAsync(_channel.Object, Delivery(Json(Valid())), ct);

        VerifyAcked(Times.Once());
        VerifyNacked(requeue: false, Times.Never());
        VerifyNacked(requeue: true, Times.Never());
    }

    [Fact]
    public async Task Handle_DuplicateEvent_AcksMessage()
    {
        var ct = TestContext.Current.CancellationToken;
        SetupRepository(false);            

        await _consumer.HandleAsync(_channel.Object, Delivery(Json(Valid())), ct);

        VerifyAcked(Times.Once());         
        VerifyNacked(requeue: true, Times.Never());
    }

    [Fact]
    public async Task Handle_MalformedJson_RejectsWithoutRequeue()
    {
        var ct = TestContext.Current.CancellationToken;

        await _consumer.HandleAsync(_channel.Object, Delivery("{ this is not json"), ct);

        VerifyNacked(requeue: false, Times.Once());
        VerifyAcked(Times.Never());
        _repository.Verify(r => r.TryAddUserProfileAsync(It.IsAny<UserProfileUpdated>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    public static TheoryData<UserProfileUpdated> InvalidEvents() => new()
    {
        Valid() with { EventId = Guid.Empty },
        Valid() with { UserId = 0 },
        Valid() with { Version = 0 },
        Valid() with { Snapshot = null! },
        Valid() with { Snapshot = new ProfileSnapshot { Name = "", Email = "a@b.com" } },
        Valid() with { Snapshot = new ProfileSnapshot { Name = "Danylo", Email = " " } }
    };

    [Theory]
    [MemberData(nameof(InvalidEvents))]
    public async Task Handle_InvalidEvent_RejectsWithoutRequeueAndSkipsRepository(UserProfileUpdated invalid)
    {
        var ct = TestContext.Current.CancellationToken;

        await _consumer.HandleAsync(_channel.Object, Delivery(Json(invalid)), ct);

        VerifyNacked(requeue: false, Times.Once());
        VerifyAcked(Times.Never());
        _repository.Verify(r => r.TryAddUserProfileAsync(It.IsAny<UserProfileUpdated>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_RepositoryThrows_NacksWithRequeue()
    {
        var ct = TestContext.Current.CancellationToken;
        _repository
            .Setup(r => r.TryAddUserProfileAsync(It.IsAny<UserProfileUpdated>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Mongo unavailable"));

        await _consumer.HandleAsync(_channel.Object, Delivery(Json(Valid())), ct);   

        VerifyNacked(requeue: true, Times.Once());
        VerifyAcked(Times.Never());
    }
    
}