using WebApiBooking.Domain.Enums;

namespace WebApiBooking.Application.DTOs;

public class RegisterUserDto
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public UserRole Role { get; set; }
}