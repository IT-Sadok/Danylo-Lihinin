using System.Diagnostics;
using ConsoleBooking.Apartment;

namespace ConsoleBooking.UI;

public class UI
{
    public ApartmentService Service { get; }
    public void Run()
    {
        while (true)
        {
            Console.WriteLine("1 - Show all hosts");
            var input = Console.ReadLine();
            if (int.TryParse(input, out int result))
            {
                switch (result)
                {
                    case 1:
                        Service.Manager.ShowAll();
                        break;
                }
            }
        }
    }
    
}