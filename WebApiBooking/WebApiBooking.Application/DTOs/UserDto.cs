namespace WebApiBooking.Application.DTOs;

public record UserDto
{
    public int id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
}