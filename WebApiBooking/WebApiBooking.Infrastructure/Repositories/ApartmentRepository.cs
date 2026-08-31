using Microsoft.EntityFrameworkCore;
using WebApiBooking.Application.Interfaces;
using WebApiBooking.Application.Models;
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

    public async Task<PagedResult<ApartmentWithHost>> GetApartmentsAsync(int pageSize, int pageNumber)
    {
        var pagedResult = new PagedResult<ApartmentWithHost>();

        pagedResult.TotalCount = await _dbContext.Apartments.CountAsync();

        pagedResult.Items = await _dbContext.Apartments
            .Skip((pageNumber - 1) * pageSize).Take(pageSize)
            .Select(a => new ApartmentWithHost
            {
                Id = a.Id,
                Name = a.Name,
                Price = a.Price,
                Rooms = a.Rooms,
                HostName = a.Host.Name
            })
            .ToListAsync();
        return pagedResult;
    }

    public async Task<Apartment?> GetApartmentByIdAsync(int id)
    {
        return await _dbContext.Apartments.SingleOrDefaultAsync(a => a.Id == id);
    }

    public async Task<PagedResult<ApartmentWithHost>> GetAvailableApartmentsAsync(DateTime startDate, DateTime endDate,
        int pageSize, int pageNumber)
    {
        var query = _dbContext.Apartments.Where(a =>
            !a.Bookings.Any(b => startDate < b.EndDate && endDate > b.StartDate));

        var pagedResult = new PagedResult<ApartmentWithHost>();
        pagedResult.TotalCount = await query.CountAsync();

        pagedResult.Items = await query
            .Skip((pageNumber - 1) * pageSize).Take(pageSize)
            .Select(a => new ApartmentWithHost
            {
                Id = a.Id,
                Name = a.Name,
                Price = a.Price,
                Rooms = a.Rooms,
                HostName = a.Host.Name
            })
            .ToListAsync();
        return pagedResult;
    }
}