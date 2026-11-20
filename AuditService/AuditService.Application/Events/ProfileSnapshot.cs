namespace AuditService.Application.Events;

public record ProfileSnapshot
{
    public string Name { get; init; } 
    public string Email { get; init; } 
}