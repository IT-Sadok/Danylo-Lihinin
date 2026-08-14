using WebApiBooking.Application.DTOs;
using WebApiBooking.Domain;

namespace WebApiBooking.Application.Interface;

public interface IBookingService
{
    public Task CreateBookingAsync(CreateBookingDto dto, int  userId);
    public Task<List<BookingResponseDto>> GetBookingsAsync(int userId);
    public Task<bool> IsApartmentAvailableAsync(int apartmentId, DateTime startDate, DateTime endDate);
}