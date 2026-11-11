using AuditService.Application.Interfaces.Services;
using AuditService.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AuditService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IUserProfileAuditService, UserProfileAuditService>();
        
        return services;
    }
}