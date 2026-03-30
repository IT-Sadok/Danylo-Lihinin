

namespace ConsoleBooking.Apartments;

public class ApartmentService
{
    public List<Apartment> Apartments = new List<Apartment>();

    public string ShowAll()
    {
            return String.Join("\n", Apartments);
    }

    public ApartmentService() { }
    public ApartmentService(List<Apartment> apartments)
    {
      Apartments = apartments;  
    }
}