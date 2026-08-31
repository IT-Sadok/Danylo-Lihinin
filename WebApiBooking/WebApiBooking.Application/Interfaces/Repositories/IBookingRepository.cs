using WebApiBooking.Domain;

namespace WebApiBooking.Application.Interfaces;

public interface IBookingRepository
{
    public Task CreateBookingAsync(Booking booking);
    public Task<List<Booking>> GetBookingByApartmentIdAsync(int apartmentId);
    public Task<List<Booking>> GetBookingByUserIdAsync(int userId);
    public Task<bool> IsApartmentAvailableAsync(int apartmentId, DateTime startDate, DateTime endDate);
}