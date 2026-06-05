using ConsoleBooking.Services.Dtos.Apartments;

namespace ConsoleBooking.Services.Dtos.Hosts;

public class HostDto
{
    public int Id { get; init; }
    public string Name { get; set; }
    public int Number { get; set; }
    public List<ApartmentDto> Apartments { get; set; }
}