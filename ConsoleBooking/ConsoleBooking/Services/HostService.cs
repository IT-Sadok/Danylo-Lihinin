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
    private int _nextId = 1;

    public HostService(IHostRepository hostRepository, IHostMapper hostMapper)
    {
        _hostRepository = hostRepository;
        _hostMapper = hostMapper;
    }

    public void AddHost(CreateHostDto hostDto)
    {
        var host = _hostMapper.MapToHost(hostDto);
        host.Id = _nextId++;
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
        var host = _hostRepository.GetHostById(hostId);
        if (host is null)
            return false;
        int index = apartmentId - 1;
        if(index < 0 || index > host.Apartments.Count)
            return false;
        host.Apartments.RemoveAt(index);
        return true;
    }

    public void AddApartment(ApartmentDto apartmentDto, int hostId)
    {
        var apartment = _hostMapper.MapToApartment(apartmentDto);
        _hostRepository.GetHostById(hostId).Apartments.Add(apartment);
    }

    public void UpdateApartment(ApartmentDto apartmentDto, int hostId, int apartmentId)
    {
        var host = _hostRepository.GetHostById(hostId);
        host.Apartments[apartmentId] = _hostMapper.MapToApartment(apartmentDto);
    }

    public void SaveAll() => _hostRepository.SaveAll();

    public void LoadAll() => _hostRepository.GetAll();
}