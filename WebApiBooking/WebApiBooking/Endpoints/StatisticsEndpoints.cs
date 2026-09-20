using WebApiBooking.Application.Interfaces;
using WebApiBooking.Domain.Constants;

namespace WebApiBooking.Endpoints;

public static class StatisticsEndpoints
{
    public static void MapStatisticsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/statistics").RequireAuthorization(policy => policy.RequireRole(Roles.Admin));
        
        group.MapGet("/average-duration", async (IStatisticsRepository repo) =>
            Results.Ok(await repo.GetAverageDurationAsync()));
        
        group.MapGet("/top-apartments", async (IStatisticsRepository repo) =>
            Results.Ok(await repo.GetTopApartmentsAsync()));
        
        group.MapGet("/active-client", async (IStatisticsRepository repo) =>
            Results.Ok(await repo.GetActiveClientAsync()));
        
        group.MapGet("/booking-duration", async (IStatisticsRepository repo) =>
            Results.Ok(await repo.GetBookingDurationAsync()));
        
        group.MapGet("/median-price", async (IStatisticsRepository repo) =>
            Results.Ok(await repo.GetMedianPriceAsync()));
    }
}