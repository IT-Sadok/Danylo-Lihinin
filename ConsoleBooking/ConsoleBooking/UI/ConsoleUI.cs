using ConsoleBooking.Models;
using ConsoleBooking.Services;
using ConsoleBooking.Services.Dtos.Apartments;
using ConsoleBooking.Services.Dtos.Hosts;


namespace ConsoleBooking.UI;

public class ConsoleUI
{
    private bool _isRunning = true;
    private HostService _service { get; }
    private ConsoleUIReader _reader = new ConsoleUIReader();

    public void Run()
    {
        _service.LoadAll();
        while (_isRunning)
        {
            Console.WriteLine();
            var input = _reader.ReadValue<int>(
                "1 - Show all hosts\n2 - Host info\n3 - Operation with hosts\n4 - Save all changes\n5 - Exit\n6 - Race condtion simulation",
                int.TryParse);

            switch (input)
            {
                case 1:
                    ShowAll();
                    break;
                case 2:
                    HostInfoById(_reader.ReadValue<int>("Write host ID: ", int.TryParse));
                    break;
                case 3:
                    OperationWithHosts();
                    break;
                case 4:
                    _service.SaveAll();
                    Console.WriteLine("Information saved!");
                    break;
                case 5:
                    _isRunning = false;
                    break;
                case 6:
                    try
                    {
                        _service.RunRaceConditionSimulation();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    break;
            }
        }
    }

    private void ShowAll()
    {
        foreach (var host in _service.GetAllHosts())
        {
            Console.WriteLine($"ID: {host.Id}, Name: {host.Name}, Number: {host.Number}");
        }
    }

    private void HostInfoById(int hostId)
    {
        var host = _service.GetHostById(hostId);
        if (host != null)
        {
            Console.WriteLine($"ID: {host.Id}, Name: {host.Name}, Number: {host.Number}");
            ApartmentInfo(host);
        }
        else
        {
            Console.WriteLine("Host is not found");
        }
    }

    private void ApartmentInfo(HostDto host)
    {
        if (host.Apartments.Count != 0)
        {
            foreach (var apartment in host.Apartments)
            {
                Console.WriteLine(
                    $"Name: {apartment.Name}, Price: {apartment.Price}, Rooms: {apartment.Rooms}, Is available: {apartment.IsAvailable}");
            }
        }
        else
        {
            Console.WriteLine("Host dont have apartments");
        }
    }

    private void OperationWithHosts()
    {
        while (true)
        {
            Console.WriteLine();
            var input = _reader.ReadValue<int>("1 - Create host, 2 - Update host, 3 - Delete host, 4 - Exit",
                int.TryParse);
            switch (input)
            {
                case 1:
                    AddHost();
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

    private void AddHost()
    {
        var host = new CreateHostDto();
        host.Name = _reader.ReadString("Write host name: ");
        host.Number = _reader.ReadValue<int>("Write host number: ", int.TryParse);
        _service.AddHost(host);
        Console.WriteLine("Host added!");
    }

    private void HostUpdate()
    {
        var hostID = _reader.ReadValue<int>("Write host ID: ", int.TryParse);
        var host = _service.GetHostById(hostID);
        if (host == null)
        {
            Console.WriteLine("Host not found!");
        }
        else
        {
            HostInfoById(hostID);
            var result = _reader.ReadValue<int>("What you want change: 1 - Name, 2 - Number, 3 - Apartments",
                int.TryParse);
            switch (result)
            {
                case 1:
                    host.Name = _reader.ReadString("Write new host name: ");
                    _service.UpdateHost(host);
                    Console.WriteLine("Host name changed!");
                    break;
                case 2:
                    host.Number = _reader.ReadValue<int>("Write new host number: ", int.TryParse);
                    _service.UpdateHost(host);
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

    private void DeleteHost()
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

    private void OperationWithApartments(HostDto host)
    {
        while (true)
        {
            var input = _reader.ReadValue<int>(
                "Choose operation: 1 - Create Apartment, 2 - Update apartment, 3 - Delete apartment, 4 - Exit ",
                int.TryParse);
            switch (input)
            {
                case 1:
                    AddApartment(host);
                    break;
                case 2:
                    ApartmentUpdate(host);
                    break;
                case 3:
                    DeleteApartment(host);
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

    private void DeleteApartment(HostDto host)
    {
        if (host.Apartments.Count > 0)
        {
            ApartmentInfo(host);
            var apartmentDeleteID = _reader.ReadValue<int>("Write apartment ID: ", int.TryParse);
            bool isDeleted = _service.RemoveApartment(host.Id, apartmentDeleteID);
            if (isDeleted)
                Console.WriteLine("Apartment deleted!");
            else
                Console.WriteLine("Apartment not deleted!");
        }
        else
        {
            Console.WriteLine("Host dont have apartments");
        }
    }

    private void AddApartment(HostDto host)
    {
        var newApartment = new ApartmentDto
        {
            Name = _reader.ReadString("Write apartment name: "),
            Price = _reader.ReadValue<decimal>("Write apartment price: ", decimal.TryParse),
            Rooms = _reader.ReadValue<short>("Write apartment rooms: ", short.TryParse),
            IsAvailable = _reader.ReadBool("Write apartment is available:")
        };
        try
        {
            _service.AddApartment(newApartment, host.Id);
            Console.WriteLine("Apartment added!");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }


    private void ApartmentUpdate(HostDto host)
    {
        if (host.Apartments.Count == 0)
        {
            Console.WriteLine("Host doesnt have apartment");
            return;
        }

        ApartmentInfo(host);
        var apartmentID = _reader.ReadValue<int>("Write apartment ID: ", int.TryParse);
        if (apartmentID < 1 || apartmentID > host.Apartments.Count)
        {
            Console.WriteLine("ID is not found");
            return;
        }

        var apartment = host.Apartments[apartmentID - 1];
        while (true)
        {
            var input = _reader.ReadValue<int>(
                "What you want change? 1 - Name, 2 - Price, 3 - Rooms, 4 - Change availability, 5 - Apply changes 6 - Exit ",
                int.TryParse);
            switch (input)
            {
                case 1:
                    apartment.Name = _reader.ReadString("Write new name: ");
                    break;
                case 2:
                    apartment.Price = _reader.ReadValue<decimal>("Write new price: ", decimal.TryParse);
                    break;
                case 3:
                    apartment.Rooms = _reader.ReadValue<short>("Write new rooms: ", short.TryParse);
                    break;
                case 4:
                    apartment.IsAvailable = _reader.ReadBool("Write apartment is available:");
                    break;
                case 5:
                    try
                    {
                        _service.UpdateApartment(apartment, host.Id, apartmentID);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }

                    break;
                case 6:
                    return;
                default:
                    Console.WriteLine("Operation not found!");
                    break;
            }
        }
    }

    public ConsoleUI(HostService service)
    {
        _service = service;
    }
}