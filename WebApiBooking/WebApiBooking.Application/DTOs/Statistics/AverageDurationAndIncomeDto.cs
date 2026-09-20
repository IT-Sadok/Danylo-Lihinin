namespace WebApiBooking.Application.DTOs.Statistics;

public record AverageDurationAndIncomeDto
{
    public int ApartmentId { get; set; }
    public string Name { get; set; }
    public TimeSpan Duration { get; set; }
    public decimal Income { get; set; }
}
