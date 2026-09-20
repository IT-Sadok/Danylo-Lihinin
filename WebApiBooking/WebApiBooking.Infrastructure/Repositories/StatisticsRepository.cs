using System.Data;
using Dapper;
using WebApiBooking.Application.DTOs.Statistics;
using WebApiBooking.Application.Interfaces;

namespace WebApiBooking.Infrastructure.Repositories;

public class StatisticsRepository : IStatisticsRepository
{
    private IDbConnectionFactory _dbConnectionFactory;
    private ISqlScriptProvider _sqlScriptProvider;

    public StatisticsRepository(IDbConnectionFactory dbConnectionFactory, ISqlScriptProvider sqlScriptProvider)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _sqlScriptProvider = sqlScriptProvider;
    }

    public async Task<List<AverageDurationAndIncomeDto>> GetAverageDurationAsync()
    {
        var sql = await _sqlScriptProvider.GetScriptAsync("GetAverageDuration");
        using var connection = _dbConnectionFactory.CreateConnection();

       var dto = await connection.QueryAsync<AverageDurationAndIncomeDto>(sql);
       return dto.ToList();
    }

    public async Task<List<TopApartmentDto>> GetTopApartmentsAsync()
    {
        var sql = await _sqlScriptProvider.GetScriptAsync("GetTopApartments");
        using var connection = _dbConnectionFactory.CreateConnection();
        
        var dto = await connection.QueryAsync<TopApartmentDto>(sql);
        return dto.ToList();
    }
    public async Task<List<ActiveClientDto>> GetActiveClientAsync()
    {
        var sql = await _sqlScriptProvider.GetScriptAsync("GetActiveClient");
        using var connection = _dbConnectionFactory.CreateConnection();
        
        var dto = await connection.QueryAsync<ActiveClientDto>(sql);
        return dto.ToList();
    }
    public async Task<BookingDurationQuantilesDto> GetBookingDurationAsync()
    {
        var sql = await _sqlScriptProvider.GetScriptAsync("GetBookingDuration");
        using var connection = _dbConnectionFactory.CreateConnection();
        
        return await connection.QuerySingleAsync<BookingDurationQuantilesDto>(sql);
    }
    public async Task<List<MedianPriceDto>> GetMedianPriceAsync()
    {
        var sql = await _sqlScriptProvider.GetScriptAsync("GetMedianPrice");
        using var connection = _dbConnectionFactory.CreateConnection();
        
        var dto =  await connection.QueryAsync<MedianPriceDto>(sql);
        return dto.ToList();
    }
}