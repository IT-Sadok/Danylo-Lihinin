using System.Text.Json;
using ConsoleBooking.Data.Interfaces;
using ConsoleBooking.Models;

namespace ConsoleBooking.Data.Repositories;

public class JsonHostRepository : IHostRepository
{
    private Dictionary<int, Host> _hosts = new Dictionary<int, Host>();
    private readonly string _filePath;

    public JsonHostRepository()
    {
        var projectDirectory = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\.."));
        _filePath = Path.Combine(projectDirectory, "Data", "Storage", "hosts.json");
    }

    public void AddHost(Host host)
    {
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
        return  _hosts.Values.ToList();
    }

    public Dictionary<int, Host> GetAllHosts()
    {
        return _hosts;
    }

    public void UpdateHost(Host host)
    {
        _hosts[host.Id] = host;
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

    public IEnumerable<Host> GetAll()
    {
        if (!File.Exists(_filePath))
        {
            return Enumerable.Empty<Host>();
        }

        var fileText = File.ReadAllText(_filePath);
        if (string.IsNullOrEmpty(fileText))
        {
            return Enumerable.Empty<Host>();
        }

        var result = JsonSerializer.Deserialize<List<Host>>(fileText);
        return result ?? Enumerable.Empty<Host>();
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