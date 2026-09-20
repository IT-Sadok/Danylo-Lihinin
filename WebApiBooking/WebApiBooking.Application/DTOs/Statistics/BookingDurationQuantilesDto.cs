namespace WebApiBooking.Application.DTOs.Statistics;

public record BookingDurationQuantilesDto
{
    public double? P25Days { get; set; }
    public double? P50Days { get; set; }
    public double? P75Days { get; set; }
    public double? P90Days { get; set; }
}