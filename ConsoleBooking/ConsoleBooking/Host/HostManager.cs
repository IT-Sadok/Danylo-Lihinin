using ConsoleBooking.Apartments;


namespace ConsoleBooking.Host;

public class HostManager  
{
    private Dictionary<int,Host> _hosts = new Dictionary<int, Host>();
    private int _nextId = 1;

    public void AddHost(string name, int number,List<Apartment> apartments)
    {
        var host = new Host
        {
            Id = _nextId,
            Name = name,
            Number = number,
            Apartments = apartments
        };
        _hosts[host.Id] = host;
        _nextId++;
    }
    public void AddHost(string name, int number)
    {
        AddHost(name, number, new List<Apartment>());
    }

    public bool HostExists(int id)
    {
       return _hosts.ContainsKey(id);
    }

    public Host? GetHostById(int id)
    {
        return HostExists(id) ?  _hosts[id] : null;
    }

    public string ShowAll()
    {
            return String.Join("\n",_hosts.Values);
    }
}