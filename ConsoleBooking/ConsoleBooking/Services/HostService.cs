using ConsoleBooking.Models;
using ConsoleBooking.Data;
using ConsoleBooking.Data.Interfaces;
using ConsoleBooking.Services.Dtos.Apartments;
using ConsoleBooking.Services.Dtos.Hosts;
using ConsoleBooking.Services.Mapping;

namespace ConsoleBooking.Services;

public class HostService
{
    private IHostMapper _hostMapper;
    private IHostRepository _hostRepository;

    public HostService(IHostRepository hostRepository, IHostMapper hostMapper)
    {
        _hostRepository = hostRepository;
        _hostMapper = hostMapper;
    }

    public void AddHost(CreateHostDto hostDto)
    {
        var host = _hostMapper.MapToHost(hostDto);
        _hostRepository.AddHost(host);
    }

    public bool DeleteHost(int id)
    {
        return _hostRepository.DeleteHostById(id);
    }

    public HostDto? GetHostById(int id)
    {
        var host = _hostRepository.GetHostById(id);
        if (host != null)
        {
           return _hostMapper.MapToHostDto(host);
        }
        return null;
    }

    public void UpdateHost(HostDto hostDto)
    {
        _hostRepository.UpdateHost(_hostMapper.MapToHost(hostDto));
    }

    public List<HostDto> GetAllHosts()
    {
        var hostList = _hostRepository.GetAllHosts();
        return hostList.Select(_hostMapper.MapToHostDto).ToList();
    }

    public bool RemoveApartment(int hostId, int apartmentId)
    {
        return _hostRepository.DeleteApartmentById(hostId,apartmentId - 1);
    }

    public void AddApartment(ApartmentDto apartmentDto, int hostId)
    {
        var apartment = _hostMapper.MapToApartment(apartmentDto);
        _hostRepository.AddApartment(apartment, hostId);
    }

    public void UpdateApartment(ApartmentDto apartmentDto, int hostId, int apartmentId)
    {
        _hostRepository.UpdateApartment(_hostMapper.MapToApartment(apartmentDto), hostId, apartmentId);
    }

    public void SaveAll() => _hostRepository.SaveAll();

    public void LoadAll() => _hostRepository.LoadAll();
}