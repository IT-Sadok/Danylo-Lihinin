using ConsoleBooking.Host;
using ConsoleBooking.Apartment;
using ConsoleBooking.UI;


ApartmentManager ApartmentsForHost1 = new ApartmentManager
{
    Apartments =
    {
        new Apartment { Name = "Holosiyv", IsAvailable = true, Price = 5000, Rooms = 1 },
        new Apartment { Name = "Khreshatik Hostel", IsAvailable = true, Price = 12500, Rooms = 2 },
        new Apartment { Name = "Podol", IsAvailable = true, Price = 15000, Rooms = 4 }
    }
};
ApartmentManager ApartmentsForHost2 = new ApartmentManager
{
    Apartments =
    {
        new Apartment { Name = "Obolon House", IsAvailable = true, Price = 7000, Rooms = 2 },
        new Apartment { Name = "Solomenskiy hostel", IsAvailable = true, Price = 6500, Rooms = 2 },
        new Apartment { Name = "Darnitsa", IsAvailable = true, Price = 5900, Rooms = 1 }
    }
};

HostManager hostManager = new HostManager();
hostManager.AddHost("Anatoliy", 664549845, ApartmentsForHost1);
hostManager.AddHost("Sergey", 504876654, ApartmentsForHost2);
ApartmentService apartmentService = new ApartmentService(hostManager);
ConsoleUI UI = new ConsoleUI(apartmentService);
   
UI.Run();


