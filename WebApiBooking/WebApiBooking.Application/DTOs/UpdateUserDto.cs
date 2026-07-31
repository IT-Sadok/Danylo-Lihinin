namespace WebApiBooking.Application.DTOs;

public record UpdateUserDto
{
    public string Name { get; set; }
    public string Email { get; set; }
}