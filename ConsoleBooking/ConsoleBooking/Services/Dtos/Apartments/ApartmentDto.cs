namespace ConsoleBooking.Services.Dtos.Apartments;

public class ApartmentDto
{
    public string Name { get; set; }
    public bool IsAvailable  { get; set; }   
    public decimal Price { get; set; }  
    public short Rooms { get; set; }
}