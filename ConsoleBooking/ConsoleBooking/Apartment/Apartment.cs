namespace ConsoleBooking.Apartment;

public class Apartment
{
    public string Name { get; set; }
    public bool IsAvailable  { get; set; }   
    public decimal Price { get; set; }  
    public short Rooms { get; set; }

    public Apartment(string name, bool isAvailable, decimal price, short rooms)
    {
        Name = name;
        IsAvailable = isAvailable;
        Price = price;
        Rooms = rooms;
    }

    public Apartment()
    {
        
    }

    public void Info()
    {
        Console.WriteLine("Name: {0}, Available: {1}, Price: {2}, Rooms: {3}", Name, IsAvailable, Price, Rooms);
    }
}