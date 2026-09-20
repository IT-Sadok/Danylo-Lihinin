using System.Data;

namespace WebApiBooking.Application.Interfaces;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}