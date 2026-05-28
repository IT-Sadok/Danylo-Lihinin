using System.Text.Json;
using ConsoleBooking.Data.Interfaces;
using ConsoleBooking.Models;

namespace ConsoleBooking.Data.Repositories;

public class JsonHostRepository : IHostRepository
{
    private readonly string _filePath;

    public JsonHostRepository(string path = @"Data\Storage\hosts.json")
    {
        var projectDirectory = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
        _filePath = Path.Combine(projectDirectory, path);
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
        return result ?? Enumerable.Empty<Host>();;
    }

    public void SaveAll(IEnumerable<Host> hosts)
    {
        hosts ??= new List<Host>();
        var json = JsonSerializer.Serialize(hosts);
        var directory = Path.GetDirectoryName(_filePath);
        if(!string.IsNullOrEmpty(directory))
            Directory.CreateDirectory(directory);
        File.WriteAllText(_filePath, json);
    }
}