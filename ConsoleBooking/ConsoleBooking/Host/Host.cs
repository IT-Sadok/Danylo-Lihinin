using ConsoleBooking.Apartments; 
namespace ConsoleBooking.Host;

public class Host
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Number { get; set; }
    public List<Apartment> Apartments = new List<Apartment>();
    
    public override string ToString()
    {
        return $"ID: {Id}, Name: {Name}, Number: {Number}";
    }

    public void AddApartment(string name, decimal price, short rooms, bool isAvailable = true)
    {
        var apartment = new Apartment
        {
            Name = name,
            Price = price,
            Rooms = rooms,
            IsAvailable = isAvailable
        };
        
        Apartments.Add(apartment);
    }

    public bool RemoveApartment(int id)
    {
        if (id >= 0 && id < Apartments.Count)
        {
            Apartments.RemoveAt(id);
            return  true;
        }
        
        return false;
    }

    public string ApartmentInfo()
    {
        return String.Join("\n", Apartments.Select((apartment, index) => $"ID: {index + 1}, {apartment.ToString()}"));
    }
}    