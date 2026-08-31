namespace WebApiBooking.Application.DTOs;

public record CreateBookingDto
{
    public int ApartmentId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}