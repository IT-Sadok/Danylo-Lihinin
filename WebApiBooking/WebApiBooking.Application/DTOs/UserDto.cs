using WebApiBooking.Domain.Enums;

namespace WebApiBooking.Application.DTOs;

public class UserDto
{
    public int id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public UserRole Role { get; set; }
}