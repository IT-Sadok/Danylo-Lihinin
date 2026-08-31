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
    }
}