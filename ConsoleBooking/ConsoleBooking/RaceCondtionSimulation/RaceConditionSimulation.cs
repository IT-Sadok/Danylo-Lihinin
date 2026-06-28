using ConsoleBooking.Data.Interfaces;
using ConsoleBooking.Data.Repositories;
using ConsoleBooking.Models;
using ConsoleBooking.Services;

namespace ConsoleBooking.RaceCondtionSimulation;

public class RaceConditionSimulation
{
    private readonly object _lockObj = new object();
    public HostService _hostService;
    public JsonHostRepository _hostRepository;

    public void Run()
    {
        var host = _hostRepository.GetHostById(1);
        if (host == null)
        {
            Console.WriteLine("No hosts found");
            return;
        }

        var apartment = host.Apartments.First();
        if (apartment == null)
        {
            Console.WriteLine("No apartments found");
            return;
        }
        SimulateWithoutLock(apartment);
        SimulateWithtLock(apartment);
    }

    public void SimulateWithoutLock(Apartment apartment)
    {
        var priceBefore = apartment.Price;
        Task task1 = Task.Run(() => IncreasePrice(apartment));
        Task task2 = Task.Run(() => IncreasePrice(apartment));
        Task.WaitAll(task1, task2);
        Console.WriteLine($"Result simulation without lock objects:");
        Console.WriteLine($"Actual Price: {apartment.Price}");
        Console.WriteLine($"Expected Price: {priceBefore + 1000}");
        Console.WriteLine("=======================================================================");
    }
    public void SimulateWithtLock(Apartment apartment)
    {
        var priceBefore = apartment.Price;
        Task task1 = Task.Run(() => IncreasePriceWithLock(apartment));
        Task task2 = Task.Run(() => IncreasePriceWithLock(apartment));
        Task.WaitAll(task1, task2);
        Console.WriteLine($"Result simulation with lock objects:");
        Console.WriteLine($"Actual Price: {apartment.Price}");
        Console.WriteLine($"Expected Price: {priceBefore + 1000}");
        Console.WriteLine("=======================================================================");
    }

    private void IncreasePrice(Apartment apartment)
    {
        for (int i = 0; i < 500; i++)
        {
            apartment.Price++;
        }
    }

    private void IncreasePriceWithLock(Apartment apartment)
    { 
        lock (_lockObj)
        {
            for (int i = 0; i < 500; i++)
            {
                apartment.Price++;
            }
        }
    }
}