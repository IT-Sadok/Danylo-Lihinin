using ConsoleBooking.Host;
using ConsoleBooking.Apartment;
using ConsoleBooking.UI;

Host host1 = new Host
{
    Name = "Anatoliy",
    Number = 552442244,
    Apartments = new ApartmentManager
    {
        Apartments =
        {
            new Apartment { Name = "Holosiyv", IsAvailable = true, Price = 5000, Rooms = 1 },
            new Apartment { Name = "Khreshatik Hostel", IsAvailable = true, Price = 12500, Rooms = 2 },
            new Apartment { Name = "Podol", IsAvailable = true, Price = 15000, Rooms = 4 }
        }
    }
};
Host host2 = new Host
{
    Name = "Sergey",
    Number = 662642244,
    Apartments = new ApartmentManager
    {
        Apartments =
        {
            new Apartment { Name = "Obolon House", IsAvailable = true, Price = 7000, Rooms = 2 },
            new Apartment { Name = "Solomenskiy hostel", IsAvailable = true, Price = 6500, Rooms = 2 },
            new Apartment { Name = "Darnitsa", IsAvailable = true, Price = 5900, Rooms = 1 }
        }
    }
};

HostManager hostManager = new HostManager
{
    Hosts =
    {
        {0,host1},
        {1,host2}
    }
    
};
ApartmentService apartmentService = new ApartmentService(hostManager);
ConsoleUI UI = new ConsoleUI(apartmentService);
   
UI.Run();


