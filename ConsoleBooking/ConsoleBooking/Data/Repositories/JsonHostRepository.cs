using System.Text.Json;
using ConsoleBooking.Data.Interfaces;
using ConsoleBooking.Models;
using System.Linq;

namespace ConsoleBooking.Data.Repositories;

public class JsonHostRepository : IHostRepository
{
    private Dictionary<int, Host> _hosts = new Dictionary<int, Host>();
    private readonly string _filePath;
    private int _nextId;

    public JsonHostRepository()
    {
        var projectDirectory = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\.."));
        _filePath = Path.Combine(projectDirectory, "Data", "Storage", "hosts.json");
    }

    public void AddHost(Host host)
    {
        host.Id = _nextId++;
        _hosts.Add(host.Id, host);
    }

    public Host? GetHostById(int id)
    {
        if (_hosts.TryGetValue(id, out var host))
            return host;
        return null;
    }

    List<Host> IHostRepository.GetAllHosts()
    {
        return _hosts.Values.ToList();
    }

    public void UpdateHost(Host host)
    {
        _hosts[host.Id] = host;
    }

    public void AddApartment(Apartment apartment, int hostId)
    {
        _hosts[hostId].Apartments.Add(apartment);
    }

    public void UpdateApartment(Apartment apartment, int hostId, int apartmentId)
    {
        _hosts[hostId].Apartments[apartmentId] = apartment;
    }

    public bool DeleteApartmentById(int hostId, int apartmentId)
    {
        if (_hosts.TryGetValue(hostId, out var host))
        {
            if (apartmentId > 0 || apartmentId < host.Apartments.Count)
            {
                if (host.Apartments[apartmentId] != null)
                {
                    host.Apartments.RemoveAt(apartmentId);
                    return true;
                }
            }
        }

        return false;
    }

    public bool DeleteHostById(int id)
    {
        if (_hosts.TryGetValue(id, out var host))
        {
            _hosts.Remove(id);
            return true;
        }

        return false;
    }

    public void LoadAll()
    {
        if (File.Exists(_filePath))
        {
            var fileText = File.ReadAllText(_filePath);
            if (!string.IsNullOrEmpty(fileText))
            {
                var result = JsonSerializer.Deserialize<List<Host>>(fileText);
                _hosts = result.ToDictionary(h => h.Id, h => h);
                _nextId = _hosts.Keys.DefaultIfEmpty(0).Max() + 1;
            }
        }
    }

    public void SaveAll()
    {
        var hosts = _hosts.Values.ToList();
        var json = JsonSerializer.Serialize(hosts);
        var directory = Path.GetDirectoryName(_filePath);
        if (!string.IsNullOrEmpty(directory))
            Directory.CreateDirectory(directory);
        File.WriteAllText(_filePath, json);
    }
}