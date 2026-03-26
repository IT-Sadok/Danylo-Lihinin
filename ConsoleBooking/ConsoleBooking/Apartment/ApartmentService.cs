using ConsoleBooking.Host;

namespace ConsoleBooking.Apartment;

public class ApartmentService // service have business logic for apartments
{
    public HostManager HostManager { get; }
    public ApartmentService(HostManager manager)
    {
        HostManager = manager;
    }

    public void Method()
    {

    }
}