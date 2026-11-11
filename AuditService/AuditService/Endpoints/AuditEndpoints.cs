using AuditService.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace AuditService.Endpoints;

public static class AuditEndpoints
{
    public static void MapAuditEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("audit").RequireAuthorization();

        group.MapGet("/users/{userId}", async ([FromRoute] int userId, [FromQuery] DateTimeOffset date, IUserProfileAuditService service, CancellationToken ct) =>
        {
            if(userId <= 0)
                return Results.BadRequest();
            
            var result = await service.GetUserProfileSnapshotAsync(userId, date,ct);
            return Results.Ok(result);
        });
    }
}