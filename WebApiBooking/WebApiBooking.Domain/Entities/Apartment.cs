namespace WebApiBooking.Domain;

public class Apartment
{
    public int Id { get; set; }
    public string? ExternalId { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public short Rooms { get; set; }

    public int HostId { get; set; }
    public User Host { get; set; }

    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}