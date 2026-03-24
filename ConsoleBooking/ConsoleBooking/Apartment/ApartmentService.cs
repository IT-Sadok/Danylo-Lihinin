using ConsoleBooking.Host;

namespace ConsoleBooking.Apartment;

public class ApartmentService // service have business logic for apartments
{
    public HostManager Manager { get; }
    public ApartmentService(HostManager manager)
    {
        Manager = manager;
    }

    public void Method()
    {

    }
}