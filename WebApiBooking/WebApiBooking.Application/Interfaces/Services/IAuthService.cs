using WebApiBooking.Application.DTOs;

namespace WebApiBooking.Application.Interface;

public interface IAuthService
{
    Task<string> LoginAsync(LoginUserDto userDto);
    Task<string> RegisterAsync(RegisterUserDto userDto);
}