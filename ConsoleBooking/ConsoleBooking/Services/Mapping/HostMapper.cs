using ConsoleBooking.Models;
using ConsoleBooking.Services.Dtos.Apartments;
using ConsoleBooking.Services.Dtos.Hosts;

namespace ConsoleBooking.Services.Mapping;

public class HostMapper : IHostMapper
{
    public Host MapToHost(CreateHostDto hostDto)
    {
        return new Host
        {
            Id = 0,
            Name = hostDto.Name,
            Number = hostDto.Number
        };
    }

    public Host MapToHost(HostDto hostDto)
    {
        return new Host
        {
            Id = hostDto.Id,
            Name = hostDto.Name,
            Number = hostDto.Number,
            Apartments = MapToApartments(hostDto.Apartments)
        };
    }

    public HostDto MapToHostDto(Host host)
    {
        return new HostDto
        {
            Id = host.Id,
            Name = host.Name,
            Number = host.Number,
            Apartments = MapToApartmentDtos(host.Apartments)
        };
    }

    public ApartmentDto MapToApartmentDto(Apartment apartment)
    {
        return new ApartmentDto
        {
            Name = apartment.Name,
            IsAvailable = apartment.IsAvailable,
            Price = apartment.Price,
            Rooms = apartment.Rooms
        };
    }

    public List<ApartmentDto> MapToApartmentDtos(List<Apartment> apartments)
    {
        return apartments.Select(MapToApartmentDto).ToList();
    }

    public List<Apartment> MapToApartments(List<ApartmentDto> apartmentDtos)
    {
        return apartmentDtos.Select(MapToApartment).ToList();
    }

    public Apartment MapToApartment(ApartmentDto apartmentDto)
    {
        return new Apartment
        {
            Name = apartmentDto.Name,
            IsAvailable = apartmentDto.IsAvailable,
            Price = apartmentDto.Price,
            Rooms = apartmentDto.Rooms
        };
    }
}