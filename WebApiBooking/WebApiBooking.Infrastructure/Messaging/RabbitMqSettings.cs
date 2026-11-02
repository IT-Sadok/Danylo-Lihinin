using System.ComponentModel.DataAnnotations;

namespace WebApiBooking.Infrastructure.Messaging;

public class RabbitMqSettings
{
    [Required] public string HostName { get; set; } = string.Empty;
    [Required] public string UserName { get; set; } = string.Empty;
    [Required] public string Password { get; set; } = string.Empty;
    [Required] public string QueueName { get; set; } = string.Empty;
}