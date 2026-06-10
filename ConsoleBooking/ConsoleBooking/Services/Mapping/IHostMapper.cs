using ConsoleBooking.Models;
using ConsoleBooking.Services.Dtos.Apartments;
using ConsoleBooking.Services.Dtos.Hosts;

namespace ConsoleBooking.Services.Mapping;

public interface IHostMapper
{
    public Host MapToHost(CreateHostDto hostDto);
    public Host MapToHost(HostDto hostDto);
    public HostDto MapToHostDto(Host host);
    public ApartmentDto MapToApartmentDto(Apartment apartment);
    public List<ApartmentDto> MapToApartmentDtos(List<Apartment> apartments);
    public Apartment MapToApartment(ApartmentDto apartmentDto);
    public List<Apartment> MapToApartments(List<ApartmentDto> apartmentDtos);
}