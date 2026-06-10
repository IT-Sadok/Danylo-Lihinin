using ConsoleBooking.Services; 
namespace ConsoleBooking.Models;

public class Host
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Number { get; set; }
    public List<Apartment> Apartments { get; set; } = new();
    
    public override string ToString()
    {
        return $"ID: {Id}, Name: {Name}, Number: {Number}";
    }
}    