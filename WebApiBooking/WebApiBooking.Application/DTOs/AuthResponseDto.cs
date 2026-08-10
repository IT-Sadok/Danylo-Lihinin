namespace WebApiBooking.Application.DTOs;

public record AuthResponseDto
{
    public string Token { get; init; }
    public string Email { get; set; }
    public string Role { get; set; }
}