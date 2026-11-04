using System.ComponentModel.DataAnnotations;

namespace AuditService.Infrastructure.Persistence.Settings;

public class MongoSettings
{
    [Required] public string ConnectionString { get; set; }
    [Required] public string DatabaseName { get; set; }
}