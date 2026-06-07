using ConsoleBooking.Models;

namespace ConsoleBooking.Data.Interfaces;

public interface IHostRepository
{
    public void AddHost(Host host);
    public Host GetHostById(int id);
    public List<Host> GetAllHosts();
    public void UpdateHost(Host host);
    public bool DeleteHostById(int id);
    public void AddApartment(Apartment apartment, int hostId);
    public void UpdateApartment(Apartment apartment, int hostId, int apartmentId);
    bool DeleteApartmentById(int hostId, int apartmentId);
    public void  LoadAll();
    
    public void SaveAll();
}