using System.Text.Json.Serialization;

namespace WebApiBooking.ConsoleApp.DTOs;

public record ApartmentExportDto
{
    public int Id { get; init; }
    public string Name { get; init; }
    [JsonRequired]public decimal Price { get; init; }
    [JsonRequired]public short Rooms { get; init; } 
}