using System.ComponentModel.DataAnnotations;

namespace AuditService.Infrastructure.Settings;

public class RedisSettings
{
    [Required] public string ConnectionString { get; init; }
    [Range(typeof(TimeSpan), "00:00:01", "00:03:00")]
    public TimeSpan ProfileTtl { get; init; }
    [Range(typeof(TimeSpan), "00:00:01", "00:01:00")]
    public TimeSpan EmptyTtl { get; init; }
}
