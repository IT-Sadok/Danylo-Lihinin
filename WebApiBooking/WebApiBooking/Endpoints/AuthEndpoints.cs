using WebApiBooking.Application.DTOs;
using WebApiBooking.Application.Interface;

namespace WebApiBooking.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoint(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/auth/");

        group.MapPost("/register", async (RegisterUserDto dto, IAuthService authService) =>
        {
            var token = await authService.RegisterAsync(dto);
            return Results.Ok(new { token });
        });

        group.MapPost("/login", async (LoginUserDto dto, IAuthService authService) =>
        {
            var token = await authService.LoginAsync(dto);
            return Results.Ok(new { token });
        });
    }
}