namespace WebApiBooking.Application.Models;

public record ApartmentWithHost
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public short Rooms { get; set; }
    
    public string HostName { get; set; }
}