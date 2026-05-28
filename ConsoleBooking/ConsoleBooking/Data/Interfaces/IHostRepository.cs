using ConsoleBooking.Models;

namespace ConsoleBooking.Data.Interfaces;

public interface IHostRepository
{
    public IEnumerable<Host>  GetAll();
    
    public void SaveAll(IEnumerable<Host> hosts);
}