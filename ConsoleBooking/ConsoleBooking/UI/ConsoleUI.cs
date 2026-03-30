using System.Diagnostics;
using ConsoleBooking.Apartments;
using ConsoleBooking.Host;

namespace ConsoleBooking.UI;

public class ConsoleUI
{
    private bool _isRunning = true;
    private HostManager _service { get; }

    public void Run()
    {
        while (_isRunning)
        {
            Console.WriteLine("1 - Show all hosts");
            Console.WriteLine("2 - Choose Host");
            Console.WriteLine("3 - Exit");
            var input = Console.ReadLine();
            if (int.TryParse(input, out int result))
            {
                switch (result)
                {
                    case 1:
                        Console.WriteLine(_service.ShowAll());
                        break;
                    case 2:
                        Console.WriteLine("Write hosts ID: ");
                        if (int.TryParse(Console.ReadLine(), out int inputId))
                        {
                            var apartmentInfo = _service.GetHostById(inputId)?.ApartmentInfo() ?? "Host is not found";
                            Console.WriteLine(apartmentInfo);
                        }

                        break;
                    case 3:
                        _isRunning = false;
                        break;
                }
            }
        }
    }

    public ConsoleUI(HostManager service)
    {
        _service = service;
    }
}