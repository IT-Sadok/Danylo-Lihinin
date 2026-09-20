using System.IdentityModel.Tokens.Jwt;
using WebApiBooking.Application.DTOs;
using WebApiBooking.Application.Interface;
using WebApiBooking.Filters;

namespace WebApiBooking.Endpoints;

public static class ApartmentEndpoints
{
    public static void MapApartmentEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/apartments", async ([AsParameters]  ApartmentFilterDto filter, IApartmentService service) =>
        {
            var apartments = await service.GetApartmentsAsync(filter);
            return Results.Ok(apartments);
        }).AddEndpointFilter<ValidationFilter<ApartmentFilterDto>>();

        app.MapPut("/apartments", async (UpsertApartmentDto dto, IApartmentService service, HttpContext httpContext) =>
        {
            var id = httpContext.User.FindFirst(JwtRegisteredClaimNames.Sub);
            if (id is null)
                return Results.NotFound();

            if (!int.TryParse(id.Value, out var hostId))
                return Results.NotFound();
            
            var  apartmentId = await service.UpsertApartmentAsync(dto, hostId);
            return Results.Ok(apartmentId);
        }).RequireAuthorization().AddEndpointFilter<ValidationFilter<UpsertApartmentDto>>();
    }
}