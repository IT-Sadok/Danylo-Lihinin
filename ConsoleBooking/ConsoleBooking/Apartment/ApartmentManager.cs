using ConsoleBooking.Interface;

namespace ConsoleBooking.Apartment;

public class ApartmentManager : IManager // Apartment manager create, delete apartments
{
    public List<Apartment> Apartments = new List<Apartment>();

    public void ShowAll()
    {
        foreach (var apartment in Apartments)
        { 
            apartment.Info();
        }
    }

    public ApartmentManager()
    {
        
    }
    public ApartmentManager(List<Apartment> apartments)
    {
      Apartments = apartments;  
    }
}