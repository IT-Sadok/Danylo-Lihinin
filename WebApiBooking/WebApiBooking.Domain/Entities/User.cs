using Microsoft.AspNetCore.Identity;

namespace WebApiBooking.Domain;

public class User : IdentityUser<int>
{
    public string Name { get; set; }
    
    public ICollection<Apartment> OwnedApartments { get; set; } = new List<Apartment>();
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
