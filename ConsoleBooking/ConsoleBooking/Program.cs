using ConsoleBooking.Host;
using ConsoleBooking.Apartments;
using ConsoleBooking.UI;



var apartment1 = new Apartment { Name = "Holosiyv", IsAvailable = true, Price = 5000, Rooms = 1 };
var apartment2 = new Apartment { Name = "Khreshatik Hostel", IsAvailable = true, Price = 12500, Rooms = 2 };
var apartment3 = new Apartment { Name = "Podol", IsAvailable = true, Price = 15000, Rooms = 4 };

var apartment4 = new Apartment { Name = "Obolon House", IsAvailable = true, Price = 7000, Rooms = 2 };
var apartment5 = new Apartment { Name = "Solomenskiy hostel", IsAvailable = true, Price = 6500, Rooms = 2 };
var apartment6 = new Apartment { Name = "Darnitsa", IsAvailable = true, Price = 5900, Rooms = 1 };

HostManager hostManager = new HostManager();
hostManager.AddHost("Anatoliy", 664549845);
hostManager.AddHost("Sergey", 504876654);
var host1 = hostManager.GetHostById(1);
host1.Apartments.Add(apartment1);
host1.Apartments.Add(apartment2);
host1.Apartments.Add(apartment3);
var host2 = hostManager.GetHostById(2);
host2.Apartments.Add(apartment4);
host2.Apartments.Add(apartment5);
host2.Apartments.Add(apartment6);
ConsoleUI UI = new ConsoleUI(hostManager);
   
UI.Run();


