using ConsoleBooking.Models;

namespace ConsoleBooking.Data.Interfaces;

public interface IHostRepository
{
    public void AddHost(Host host);
    public Host GetHostById(int id);
    public List<Host> GetAllHosts();
    public void UpdateHost(Host host);
    public bool DeleteHostById(int id);
    public IEnumerable<Host>  GetAll();
    
    public void SaveAll();
}