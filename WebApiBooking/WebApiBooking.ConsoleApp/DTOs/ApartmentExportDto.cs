namespace WebApiBooking.ConsoleApp.DTOs;

public record ApartmentExportDto
{
    public int Id { get; init; }
    public string Name { get; init; }
    public decimal Price { get; init; }
    public short Rooms { get; init; }
}