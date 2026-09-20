namespace WebApiBooking.Application.DTOs.Statistics;

public record TopApartmentDto
{
    public int ApartmentId { get; set; }
    public string Name { get; set; }
    public int BookingCount { get; set; }
}