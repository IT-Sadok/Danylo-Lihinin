using System.Diagnostics;
using ConsoleBooking.Apartments;
using ConsoleBooking.Host;

namespace ConsoleBooking.UI;

public class ConsoleUI
{
    private bool _isRunning = true;
    private HostManager _service { get; }
    private ConsoleUIReader _reader = new ConsoleUIReader();

    public void Run()
    {
        while (_isRunning)
        {
            Console.WriteLine();
            var input = _reader.ReadValue<int>("1 - Show all hosts\n2 - Host info\n3 - Operation with hosts\n4 - Exit",
                int.TryParse);

            switch (input)
            {
                case 1:
                    Console.WriteLine(_service.ShowAll());
                    break;
                case 2:
                    int inputId = _reader.ReadValue<int>("Write hosts ID: ", int.TryParse);
                    var apartmentInfo = _service.GetHostById(inputId)?.ApartmentInfo() ?? "Host is not found";
                    Console.WriteLine(apartmentInfo);
                    break;
                case 3:
                    OperationWithHosts();
                    break;
                case 4:
                    _isRunning = false;
                    break;
            }
        }
    }

    void OperationWithHosts()
    {
        while (true)
        {
            Console.WriteLine();
            var input = _reader.ReadValue<int>("1 - Create host, 2 - Update host, 3 - Delete host, 4 - Exit",
                int.TryParse);
            switch (input)
            {
                case 1:
                    var name = _reader.ReadString("Write host name: ");
                    var number = _reader.ReadValue<int>("Write host number: ", int.TryParse);
                    _service.AddHost(name, number);
                    Console.WriteLine("Host added!");
                    break;
                case 2:
                    HostUpdate();
                    break;
                case 3:
                    DeleteHost();
                    break;
                case 4:
                    return;
            }
        }
    }

    void HostUpdate()
    {
        var hostID = _reader.ReadValue<int>("Write host ID: ", int.TryParse);
        var host = _service.GetHostById(hostID);
        if (host == null)
        {
            Console.WriteLine("Host not found!");
        }
        else
        {
            Console.WriteLine(host.ToString());
            var result = _reader.ReadValue<int>("What you want change: 1 - Name, 2 - Number, 3 - Apartments",
                int.TryParse);
            switch (result)
            {
                case 1:
                    var newName = _reader.ReadString("Write new host name: ");
                    host.Name = newName;
                    Console.WriteLine("Host name changed!");
                    break;
                case 2:
                    var newNumber = _reader.ReadValue<int>("Write new host number: ", int.TryParse);
                    host.Number = newNumber;
                    Console.WriteLine("Host number changed!");
                    break;
                case 3:
                    OperationWithApartments(host);
                    break;
                default:
                    Console.WriteLine("Operation not found!");
                    break;
            }
        }
    }

    void DeleteHost()
    {
        var hostID = _reader.ReadValue<int>("Write host ID: ", int.TryParse);
        var isDeleted = _service.DeleteHost(hostID);
        if (isDeleted)
        {
            Console.WriteLine("Host deleted!");
        }
        else
        {
            Console.WriteLine("Host not found");
        }
    }

    void OperationWithApartments(Host.Host host)
    {
        while (true)
        {
            var input = _reader.ReadValue<int>(
                "Choose operation: 1 - Create Apartment, 2 - Update apartment, 3 - Delete apartment, 4 - Exit ",
                int.TryParse);
            switch (input)
            {
                case 1:
                    var apartmentName = _reader.ReadString("Write apartment name: ");
                    var apartmentPrice = _reader.ReadValue<decimal>("Write apartment price: ", decimal.TryParse);
                    var apartmentRooms = _reader.ReadValue<short>("Write apartment rooms: ", short.TryParse);
                    var apartmentIsAvailable = _reader.ReadBool("Write apartment is available:");
                    host.AddApartment(apartmentName, apartmentPrice, apartmentRooms, apartmentIsAvailable);
                    Console.WriteLine("Apartment added!");
                    break;
                case 2:
                    if (host.Apartments.Count > 0)
                    {
                        Console.WriteLine(host.ApartmentInfo());
                        var apartmentID = _reader.ReadValue<int>("Write apartment ID: ", int.TryParse);
                        if (apartmentID > 0 && apartmentID <= host.Apartments.Count)
                        {
                            var apartmentUpdate = host.Apartments[apartmentID - 1];
                            ApartmentUpdate(apartmentUpdate);
                        }
                        else
                            Console.WriteLine("ID is not  found!");
                    }
                    else
                    {
                        Console.WriteLine("Host dont have apartments");
                    }

                    break;
                case 3:
                    if (host.Apartments.Count > 0)
                    {
                        Console.WriteLine(host.ApartmentInfo());
                        var apartmentDeleteID = _reader.ReadValue<int>("Write apartment ID: ", int.TryParse);
                        var isDeleted = host.RemoveApartment(apartmentDeleteID - 1);
                        if (isDeleted)
                            Console.WriteLine("Apartment deleted!");
                        else
                            Console.WriteLine("Apartment not deleted!");
                    }
                    else
                    {
                        Console.WriteLine("Host dont have apartments");
                    }
                    break;
                case 4:
                    Console.WriteLine("Operation complete");
                    return;
                default:
                    Console.WriteLine("Operation not found!");
                    break;
            }
        }
    }


    void ApartmentUpdate(Apartment apartment)
    {
        while (true)
        {
            var input = _reader.ReadValue<int>(
                "What you want change? 1 - Name, 2 - Price, 3 - Rooms, 4 - Change availability, 5 - Exit ",
                int.TryParse);
            switch (input)
            {
                case 1:
                    var name = _reader.ReadString("Write new name: ");
                    apartment.Name = name;
                    Console.WriteLine("Apartment name changed!");
                    break;
                case 2:
                    var price = _reader.ReadValue<decimal>("Write new price: ", decimal.TryParse);
                    apartment.Price = price;
                    Console.WriteLine("Apartment price changed!");
                    break;
                case 3:
                    var rooms = _reader.ReadValue<short>("Write new rooms: ", short.TryParse);
                    apartment.Rooms = rooms;
                    Console.WriteLine("Apartment rooms changed!");
                    break;
                case 4:
                    var isAvailable = _reader.ReadBool("Write apartment is available:");
                    apartment.IsAvailable = isAvailable;
                    Console.WriteLine("Apartment is available changed!");
                    break;
                case 5:
                    return;
                default:
                    Console.WriteLine("Operation not found!");
                    break;
            }
        }
    }

    public ConsoleUI(HostManager service)
    {
        _service = service;
    }
}