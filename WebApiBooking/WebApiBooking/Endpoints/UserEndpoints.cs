using System.IdentityModel.Tokens.Jwt;
using WebApiBooking.Application.DTOs;
using WebApiBooking.Application.Interface;
using WebApiBooking.Application.Services;
using WebApiBooking.Filters;

namespace WebApiBooking.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("users").RequireAuthorization();

        group.MapPut("/me", async (UpdateUserDto dto, HttpContext httpContext, IUserService userService) =>
        {
            var id = httpContext.User.FindFirst(JwtRegisteredClaimNames.Sub);

            if (id is null)
                return Results.NotFound();

            if (!int.TryParse(id.Value, out var userId))
                return Results.NotFound();

            await userService.UpdateProfileAsync(dto, userId);
            return Results.Ok();
        }).AddEndpointFilter<ValidationFilter<UpdateUserDto>>();
    }
}