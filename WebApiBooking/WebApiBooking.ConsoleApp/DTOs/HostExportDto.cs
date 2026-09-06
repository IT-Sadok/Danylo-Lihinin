namespace WebApiBooking.ConsoleApp.DTOs;

public record HostExportDto
{
    public int Id { get; init; }
    public string Name { get; init; }
    public string Email { get; init; }
    
    public List<ApartmentExportDto> Apartments { get; init; }
}
    
