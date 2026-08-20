using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.JsonWebTokens;
using WebApiBooking.Application.DTOs;
using WebApiBooking.Application.Interface;
using WebApiBooking.Domain;
using WebApiBooking.Filters;

namespace WebApiBooking.Endpoints;

public static class BookingEndpoints
{
    public static void MapBookingEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/bookings");

        group.MapPost("", async (CreateBookingDto dto, HttpContext httpContext, IBookingService bookingService) =>
        {
            var id = httpContext.User.FindFirst(JwtRegisteredClaimNames.Sub);

            if (id is null)
                return Results.Unauthorized();

            if (!int.TryParse(id.Value, out var userId))
                return Results.Unauthorized();

            await bookingService.CreateBookingAsync(dto, userId);
            return Results.Created();
        }).RequireAuthorization().AddEndpointFilter<ValidationFilter<CreateBookingDto>>();

        group.MapGet("/my", async (HttpContext httpContext, IBookingService bookingService) =>
        {
            var id = httpContext.User.FindFirst(JwtRegisteredClaimNames.Sub);

            if (id is null)
                return Results.Unauthorized();

            if (!int.TryParse(id.Value, out var userId))
                return Results.Unauthorized();
            
            var bookings = await bookingService.GetBookingsAsync(userId);
            return Results.Ok(bookings);
        }).RequireAuthorization();
    }
}