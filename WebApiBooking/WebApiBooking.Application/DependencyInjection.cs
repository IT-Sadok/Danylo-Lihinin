using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using WebApiBooking.Application.Interface;
using WebApiBooking.Application.Services;
using WebApiBooking.Application.Validators;

namespace WebApiBooking.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IUserService,UserService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddValidatorsFromAssemblyContaining<RegisterUserDtoValidator>();
        
        return services;
    }
}