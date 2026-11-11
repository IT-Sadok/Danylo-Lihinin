using System.Text.Json;
using AuditService.Application.Events;
using AuditService.Application.Interfaces.Repositories;
using AuditService.Infrastructure.Settings;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace AuditService.Infrastructure.Messaging;

public class ProfileEventsConsumer : BackgroundService
{
    private readonly IUserProfileHistoryRepository _repository;
    private readonly ILogger<ProfileEventsConsumer> _logger;
    private readonly RabbitMqSettings _settings;

    public ProfileEventsConsumer(IUserProfileHistoryRepository userProfileHistoryRepository,
        ILogger<ProfileEventsConsumer> logger, IOptions<RabbitMqSettings> rabbitMqSettings)
    {
        _repository = userProfileHistoryRepository;
        _logger = logger;
        _settings = rabbitMqSettings.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory
        {
            HostName = _settings.HostName,
            UserName = _settings.UserName,
            Password = _settings.Password
        };
        
        await using var connection = await factory.CreateConnectionAsync(stoppingToken);
        await using var channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);
        await channel.QueueDeclareAsync(queue: _settings.QueueName, durable: true,exclusive: false, autoDelete: false,
            arguments: null, cancellationToken: stoppingToken);
        await channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 10, global: false, cancellationToken: stoppingToken);
        
        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += (sender, ea) => HandleAsync(channel,ea, stoppingToken);
        await channel.BasicConsumeAsync(queue: _settings.QueueName, autoAck: false, 
            consumer: consumer, cancellationToken: stoppingToken);
        await Task.Delay(Timeout.Infinite, stoppingToken);
        
    }

    private async Task HandleAsync(IChannel channel, BasicDeliverEventArgs ea, CancellationToken stoppingToken)
    {
        try
        {
            var userEvent = JsonSerializer.Deserialize<UserProfileUpdated>(ea.Body.Span);
            if (userEvent is null || userEvent.EventId == Guid.Empty || userEvent.UserId <= 0
                || userEvent.Version <= 0 || userEvent.Snapshot is null)
            {
                _logger.LogWarning("Invalid event {MessageId}, rejecting", ea.BasicProperties.MessageId);
                await channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: false, stoppingToken);
                return;
            }

            var added = await _repository.TryAddUserProfileAsync(userEvent, stoppingToken);
            _logger.LogInformation("Event {EventId} {Result}", userEvent.EventId, added ? "stored" : "duplicate");
            await channel.BasicAckAsync(ea.DeliveryTag, false, stoppingToken);
        }
        catch (JsonException ex)
        {
            _logger.LogWarning(ex, "Malformed JSON in {MessageId}, rejecting", ea.BasicProperties.MessageId);
            await channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: false, stoppingToken);

        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "Operation cancelled");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Temporary failure for {MessageId}, will retry", ea.BasicProperties.MessageId);
            await Task.Delay(5000, stoppingToken);
            await channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: true, stoppingToken);

        }
    }
    
}
    
    