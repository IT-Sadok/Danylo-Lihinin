namespace ConsoleBooking.Classes;

public class Host
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Number { get; set; }
    public ApartmentManager Apartments { get; set; } = new ApartmentManager();

    public Host(int id, string name, int number)
    {
        Id = id;
        Name = name;
        Number = number;
    }

    public Host()
    {
        
    }

    public void Info()
    {
        Console.WriteLine("ID: {0}, Name: {1}, Number: {2}", Id, Name, Number);
    }

    public void ApartmentInfo()
    {
        foreach (var apartment in Apartments.Apartments)
        {
            apartment.Info();
        }
    }
}    