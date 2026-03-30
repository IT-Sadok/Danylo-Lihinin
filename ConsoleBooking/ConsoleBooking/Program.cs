using ConsoleBooking.Host;
using ConsoleBooking.Apartments;
using ConsoleBooking.UI;


List<Apartment> ApartmentsForHost1 = new()
{
        new Apartment { Name = "Holosiyv", IsAvailable = true, Price = 5000, Rooms = 1 },
        new Apartment { Name = "Khreshatik Hostel", IsAvailable = true, Price = 12500, Rooms = 2 },
        new Apartment { Name = "Podol", IsAvailable = true, Price = 15000, Rooms = 4 }
};
List<Apartment> ApartmentsForHost2 = new()
{
        new Apartment { Name = "Obolon House", IsAvailable = true, Price = 7000, Rooms = 2 },
        new Apartment { Name = "Solomenskiy hostel", IsAvailable = true, Price = 6500, Rooms = 2 },
        new Apartment { Name = "Darnitsa", IsAvailable = true, Price = 5900, Rooms = 1 }
};

HostManager hostManager = new HostManager();
hostManager.AddHost("Anatoliy", 664549845,ApartmentsForHost1);
hostManager.AddHost("Sergey", 504876654,ApartmentsForHost2);
ConsoleUI UI = new ConsoleUI(hostManager);
   
UI.Run();


