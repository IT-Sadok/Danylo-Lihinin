using ConsoleBooking.Interface;

namespace ConsoleBooking.Classes;

public class ApartmentManager : IManager
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