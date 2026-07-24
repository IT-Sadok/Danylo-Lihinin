using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApiBooking.Application.Interface;
using WebApiBooking.Domain.Interfaces;
using WebApiBooking.Infrastructure.Persistence;
using WebApiBooking.Infrastructure.Repositories;

namespace WebApiBooking.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<BookingDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
        });
        services.AddScoped<IUserRepository,UserRepository>();
        services.AddScoped<IPasswordHasher,BCryptPasswordHasher>();
        services.AddScoped<IJwtService, JwtService>();
        return services;
    }
}