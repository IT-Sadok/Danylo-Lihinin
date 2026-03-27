using ConsoleBooking.Apartment;
using ConsoleBooking.Interface;

namespace ConsoleBooking.Host;

public class HostManager : IManager
{
    public Dictionary<int,Host> Hosts = new Dictionary<int, Host>();
    private int _nextId = 1;

    public void AddHost(string name, int number, ApartmentManager apartmentManager)
    {
        var host = new Host
        {
            ID = _nextId,
            Name = name,
            Number = number,
            Apartments = apartmentManager
        };
        Hosts[host.ID] = host;
        _nextId++;
    }
    public void ShowAll()
    {
        foreach (var host in Hosts)
        {
            host.Value.Info();
        }
    }
}