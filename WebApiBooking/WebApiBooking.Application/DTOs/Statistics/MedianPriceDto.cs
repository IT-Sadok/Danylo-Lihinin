namespace WebApiBooking.Application.DTOs.Statistics;

public record MedianPriceDto
{
    public int HostId { get; set; }
    public string HostName { get; set; }
    public int ApartmentCount { get; set; }
    public decimal MedianPrice { get; set; }
    public decimal P90Price { get; set; }
}