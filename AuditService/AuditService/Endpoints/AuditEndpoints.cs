using AuditService.Application.Interfaces.Services;

namespace AuditService.Endpoints;

public static class AuditEndpoints
{
    public static void MapAuditEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("audit").RequireAuthorization(policy => policy.RequireRole("Admin"));

        group.MapGet("/users/{userId}", async (int userId, DateTimeOffset at, IUserProfileAuditService service, CancellationToken ct) =>
        {
            if(userId <= 0)
                return Results.BadRequest("userId must be greater than zero");
            
            var result = await service.GetUserProfileSnapshotAsync(userId, at,ct);

            return result is null ? Results.NotFound() : Results.Ok(result);
        });
    }
}