namespace ConsoleBooking.Apartments;

public class Apartment
{
    public string Name { get; set; }
    public bool IsAvailable  { get; set; }   
    public decimal Price { get; set; }  
    public short Rooms { get; set; }

    public override string ToString()
    {
        return $"Name: {Name}, Available: {IsAvailable}, Price: {Price}, Rooms: {Rooms}";
    }
}