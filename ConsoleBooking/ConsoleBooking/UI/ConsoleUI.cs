using System.Diagnostics;
using ConsoleBooking.Apartment;

namespace ConsoleBooking.UI;

public class ConsoleUI
{
    private  bool _isRunning = true;
    public ApartmentService Service { get; }
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
                        Service.HostManager.ShowAll();
                        break;
                    case 2:
                        Console.WriteLine("Write hosts ID: ");
                        int inputId = int.Parse(Console.ReadLine());
                        if (Service.HostManager.Hosts.ContainsKey(inputId))
                        {
                            Service.HostManager.Hosts[inputId].ApartmentInfo();

                        }
                        else
                        {
                            Console.WriteLine("No hosts found");
                        }
                        break;
                    case 3:
                        _isRunning =false;
                        break;
                }
            }
        }
    }
    public ConsoleUI(ApartmentService service)
        {
        Service = service;
        }
}