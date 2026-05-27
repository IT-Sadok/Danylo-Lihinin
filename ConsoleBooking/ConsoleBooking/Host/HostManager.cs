using ConsoleBooking.Apartments;


namespace ConsoleBooking.Host;

public class HostManager
{
    private Dictionary<int, Host> _hosts = new Dictionary<int, Host>();
    private int _nextId = 1;

    public void AddHost(string name, int number)
    {
        var host = new Host
        {
            Id = _nextId,
            Name = name,
            Number = number,
        };
        _hosts[host.Id] = host;
        _nextId++;
    }

    public bool DeleteHost(int id)
    {
        if (HostExists(id))
        {
            _hosts.Remove(id);
            return true;
        }
        
        return false;
    }

    public bool HostExists(int id)
    {
        return _hosts.ContainsKey(id);
    }

    public Host? GetHostById(int id)
    {
        return HostExists(id) ? _hosts[id] : null;
    }

    public string ShowAll()
    {
        return String.Join("\n", _hosts.Values);
    }
}