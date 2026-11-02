using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using WebApiBooking.Infrastructure.Persistence;

namespace WebApiBooking.Infrastructure.Messaging;

public class OutboxPublisherWorker : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<OutboxPublisherWorker> _logger;
    private readonly RabbitMqSettings _settings;

    public OutboxPublisherWorker(IServiceScopeFactory scopeFactory, ILogger<OutboxPublisherWorker> logger, IOptions<RabbitMqSettings> settings)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
        _settings = settings.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await PublishPendingAsync(stoppingToken);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex,"Outbox publishing failed, will retry");
            }

            Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }
    }

    private async Task PublishPendingAsync(CancellationToken ct)
    {
        using var scope = _scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<BookingDbContext>();

        var messages = await dbContext.OutboxMessages.Where(m => m.ProcessedAt == null)
            .OrderBy(m => m.CreatedAt)
            .Take(50)
            .ToListAsync(ct);

        if (messages.Count == 0)
            return;

        var factory = new ConnectionFactory()
        {
            HostName = _settings.HostName,
            UserName = _settings.UserName,
            Password = _settings.Password,
        };

        await using var connection = await factory.CreateConnectionAsync(ct);
        var channelOptions = new CreateChannelOptions(publisherConfirmationsEnabled: true,
            publisherConfirmationTrackingEnabled: true);
        await using var channel = await connection.CreateChannelAsync(channelOptions, ct);
        await channel.QueueDeclareAsync(queue: _settings.QueueName, durable: true, exclusive: false, autoDelete: false,
            arguments: null, cancellationToken: ct);

         foreach (var message in messages)
         {
             var props = new BasicProperties
             {
                 Persistent = true,
                 MessageId = message.Id.ToString(),
                 Type = message.Type,
                 ContentType = "application/json",
             };
             await channel.BasicPublishAsync(
                 exchange: string.Empty,
                 routingKey: _settings.QueueName,
                 mandatory: false,
                 basicProperties: props,
                 body: Encoding.UTF8.GetBytes(message.Payload),
                 cancellationToken: ct
             );
             
             message.ProcessedAt = DateTime.UtcNow;
             await dbContext.SaveChangesAsync(ct);
         }
    }
}