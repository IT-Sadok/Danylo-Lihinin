using Microsoft.EntityFrameworkCore;
using WebApiBooking.Application.Interfaces;
using WebApiBooking.Domain;
using WebApiBooking.Infrastructure.Persistence;

namespace WebApiBooking.Infrastructure.Repositories;

public class BookingRepository : IBookingRepository
{
    private readonly BookingDbContext _dbContext;
    
    public BookingRepository(BookingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreateBookingAsync(Booking booking)
    {
        await _dbContext.Bookings.AddAsync(booking);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<Booking>> GetBookingByApartmentIdAsync(int apartmentId)
    {
        return await _dbContext.Bookings.Where(b => b.ApartmentId == apartmentId).ToListAsync();
    }

    public async Task<List<Booking>> GetBookingByUserIdAsync(int userId)
    {
        return await  _dbContext.Bookings.Where(b => b.UserId == userId).Include(b => b.Apartment).ToListAsync();
    }

    public async Task<bool> IsApartmentAvailableAsync(int apartmentId, DateTime startDate, DateTime endDate)
    {
        var hasOverLap = await _dbContext.Bookings.AnyAsync(b => b.ApartmentId == apartmentId && startDate < b.EndDate && endDate > b.StartDate);
        return !hasOverLap;
    }
}