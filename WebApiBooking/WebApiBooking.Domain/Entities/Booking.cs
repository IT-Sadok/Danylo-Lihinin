namespace WebApiBooking.Domain;

public class Booking
{
    public int Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    
    public int UserId { get; set; }
    public User User { get; set; }
    
    public int ApartmentId { get; set; }
    public Apartment Apartment { get; set; }
}