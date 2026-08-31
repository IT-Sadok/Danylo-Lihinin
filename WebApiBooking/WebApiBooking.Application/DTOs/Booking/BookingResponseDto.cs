namespace WebApiBooking.Application.DTOs;

public record BookingResponseDto
{
    public int Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string ApartmentName { get; set; }
}