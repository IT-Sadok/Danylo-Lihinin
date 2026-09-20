namespace WebApiBooking.Application.DTOs.Statistics;

public record ActiveClientDto
{
    public int UserId { get; set; }
    public string Name { get; set; }
    public int TotalBookings { get; set; }
    public decimal TotalSpent { get; set; }
}