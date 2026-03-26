using ConsoleBooking.Apartment;
namespace ConsoleBooking.Host;

public class Host
{
    public string Name { get; set; }
    public int Number { get; set; }
    public ApartmentManager Apartments { get; set; } = new ApartmentManager();

    public Host(int id, string name, int number)
    {
        Name = name;
        Number = number;
    }

    public Host()
    {
        
    }

    public void Info(int id)
    {
        Console.WriteLine($"ID: {id}, Name: {Name}, Number: {Number}");
        
    }

    public void ApartmentInfo()
    {
        foreach (var apartment in Apartments.Apartments)
        {
            apartment.Info();
        }
    }
}    