using ConsoleBooking.Interface;

namespace ConsoleBooking.Host;

public class HostManager : IManager
{
    public Dictionary<int,Host> Hosts = new Dictionary<int, Host>();

    public void ShowAll()
    {
        foreach (var host in Hosts)
        {
            host.Value.Info(host.Key);
        }
    }
}