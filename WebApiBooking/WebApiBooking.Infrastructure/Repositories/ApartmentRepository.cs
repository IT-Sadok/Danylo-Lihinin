using Dapper;
using Microsoft.EntityFrameworkCore;
using WebApiBooking.Application.DTOs;
using WebApiBooking.Application.Interfaces;
using WebApiBooking.Application.Models;
using WebApiBooking.Domain;
using WebApiBooking.Infrastructure.Persistence;

namespace WebApiBooking.Infrastructure.Repositories;

public class ApartmentRepository : IApartmentRepository
{
    private BookingDbContext _dbContext;
    private ISqlScriptProvider _sqlScriptProvider;
    private IDbConnectionFactory _dbConnectionFactory;

    public ApartmentRepository(BookingDbContext dbContext, ISqlScriptProvider sqlScriptProvider, IDbConnectionFactory dbConnectionFactory)
    {
        _dbContext = dbContext;
        _sqlScriptProvider = sqlScriptProvider;
        _dbConnectionFactory = dbConnectionFactory;
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

    public async Task<int> UpsertApartmentAsync(UpsertApartmentDto dto, int currentHostId)
    {
        var sql = await _sqlScriptProvider.GetScriptAsync("UpsertApartment");
        using var connection = _dbConnectionFactory.CreateConnection();
        
        return await connection.ExecuteScalarAsync<int>(sql, new
        {
            dto.Id,
            dto.Name,
            dto.Price,
            dto.Rooms,
            dto.CustomData,
            HostId = currentHostId
        });
    }
}