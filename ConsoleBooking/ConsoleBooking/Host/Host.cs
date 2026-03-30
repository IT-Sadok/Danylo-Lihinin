using ConsoleBooking.Apartments; 
namespace ConsoleBooking.Host;

public class Host
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Number { get; set; }
    public List<Apartment> Apartments { get; set; }
    
    public override string ToString()
    {
        return $"ID: {Id}, Name: {Name}, Number: {Number}";
    }

    public string ApartmentInfo()
    {
        return String.Join("\n", Apartments);
    }
}    