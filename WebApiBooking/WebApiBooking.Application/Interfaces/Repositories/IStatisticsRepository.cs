using WebApiBooking.Application.DTOs.Statistics;
using WebApiBooking.Domain;

namespace WebApiBooking.Application.Interfaces;

public interface IStatisticsRepository
{
    public Task<List<AverageDurationAndIncomeDto>> GetAverageDurationAsync();
    public Task<List<TopApartmentDto>> GetTopApartmentsAsync();
    public Task<List<ActiveClientDto>> GetActiveClientAsync();
    public Task<BookingDurationQuantilesDto> GetBookingDurationAsync();
    public Task<List<MedianPriceDto>> GetMedianPriceAsync();
}