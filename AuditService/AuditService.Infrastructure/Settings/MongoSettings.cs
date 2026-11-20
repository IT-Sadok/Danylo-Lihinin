using System.ComponentModel.DataAnnotations;

namespace AuditService.Infrastructure.Settings;

public class MongoSettings
{
    [Required] public string ConnectionString { get; set; }
    [Required] public string DatabaseName { get; set; }
}