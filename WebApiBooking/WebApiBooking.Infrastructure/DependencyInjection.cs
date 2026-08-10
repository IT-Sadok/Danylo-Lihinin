using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApiBooking.Application.Interface;
using WebApiBooking.Application.Settings;
using WebApiBooking.Domain;
using WebApiBooking.Infrastructure.Persistence;

namespace WebApiBooking.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<BookingDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
        });
        
        services.AddIdentity<User,IdentityRole<int>>(options =>
        {
            options.Password.RequiredLength = 6;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.User.RequireUniqueEmail = true;
        }).AddEntityFrameworkStores<BookingDbContext>().AddDefaultTokenProviders();
        
        services.AddScoped<IJwtService, JwtService>();
        
        return services;
    }
}