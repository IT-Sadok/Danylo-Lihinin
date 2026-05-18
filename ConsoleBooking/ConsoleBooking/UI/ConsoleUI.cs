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
            var input = _reader.ReadValue<int>("1 - Show all hosts\n2 - Choose Host\n3 - Exit", int.TryParse);

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
                    _isRunning = false;
                    break;
            }
        }
    }

    public ConsoleUI(HostManager service)
    {
        _service = service;
    }
}