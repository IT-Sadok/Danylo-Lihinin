using System.Data;
using Microsoft.Extensions.Configuration;
using Npgsql;
using WebApiBooking.Application.Interfaces;

namespace WebApiBooking.Infrastructure;

public class NpgsqlConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public NpgsqlConnectionFactory(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");
    }

    public IDbConnection CreateConnection() => new NpgsqlConnection( _connectionString);
}