using ConsoleBooking.Host;
using ConsoleBooking.Apartment;


Host host1 = new Host
{
    Id = 1,
    Name = "Anatoliy",
    Number = 552442244,
    Apartments = new ApartmentManager
    {
        Apartments =
        {
            new Apartment { Name = "Garden Hills", IsAvailable = true, Price = 5000, Rooms = 3 },
            new Apartment { Name = "Khreshatik Hostel", IsAvailable = true, Price = 5500, Rooms = 2 },
            new Apartment { Name = "Podol", IsAvailable = true, Price = 15000, Rooms = 4 }
        }
    }
};
Host host2 = new Host
{
    Id = 2,
    Name = "Sergey",
    Number = 662642244,
    Apartments = new ApartmentManager
    {
        Apartments =
        {
            new Apartment { Name = "Garden Hills", IsAvailable = true, Price = 5000, Rooms = 3 },
            new Apartment { Name = "Khreshatik Hostel", IsAvailable = true, Price = 5500, Rooms = 2 },
            new Apartment { Name = "Podol", IsAvailable = true, Price = 15000, Rooms = 4 }
        }
    }
};

HostManager hostManager = new HostManager
{
    Hosts =
    {
        host1,
        host2
    }
    
};



