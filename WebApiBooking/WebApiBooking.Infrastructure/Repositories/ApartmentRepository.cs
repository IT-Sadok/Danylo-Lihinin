using Microsoft.EntityFrameworkCore;
using WebApiBooking.Application.Interfaces;
using WebApiBooking.Domain;
using WebApiBooking.Infrastructure.Persistence;

namespace WebApiBooking.Infrastructure.Repositories;

public class ApartmentRepository : IApartmentRepository
{
    private BookingDbContext _dbContext;

    public ApartmentRepository(BookingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Apartment>> GetApartmentsAsync()
    {
        return await _dbContext.Apartments.Include(a => a.Host).ToListAsync();
    }

    public async Task<Apartment?> GetApartmentByIdAsync(int id)
    {
        return await _dbContext.Apartments.SingleOrDefaultAsync(a => a.Id == id);
    }
}