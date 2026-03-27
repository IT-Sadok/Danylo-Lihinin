using ConsoleBooking.Apartment;
namespace ConsoleBooking.Host;

public class Host
{
    public int ID { get; set; }
    public string Name { get; set; }
    public int Number { get; set; }
    public ApartmentManager Apartments { get; set; } = new ApartmentManager();

    public Host(string name, int number)
    {
        Name = name;
        Number = number;
    }

    public Host() { }

    public void Info()
    {
        Console.WriteLine($"ID: {ID}, Name: {Name}, Number: {Number}");
        
    }

    public void ApartmentInfo()
    {
        foreach (var apartment in Apartments.Apartments)
        {
            apartment.Info();
        }
    }
}    