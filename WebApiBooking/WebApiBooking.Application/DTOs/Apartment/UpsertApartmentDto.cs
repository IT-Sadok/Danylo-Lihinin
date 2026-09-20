namespace WebApiBooking.Application.DTOs;

public record UpsertApartmentDto
{
    public int? Id { get; set; }
    
    public string Name { get; set; }
    public decimal Price { get; set; }
    public short Rooms { get; set; }
    public string? CustomData { get; set; }
}