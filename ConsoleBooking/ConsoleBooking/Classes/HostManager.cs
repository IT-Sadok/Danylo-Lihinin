using ConsoleBooking.Interface;

namespace ConsoleBooking.Classes;

public class HostManager : IManager
{
    public List<Host> Hosts = new List<Host>();

    public void ShowAll()
    {
        foreach (var host in Hosts)
        {
            host.Info();
        }
    }
}