using ConsoleBooking.Data.Interfaces;
using ConsoleBooking.Data.Repositories;
using ConsoleBooking.Models;
using ConsoleBooking.Services;

namespace ConsoleBooking.RaceCondtionSimulation;

public class RaceConditionSimulation
{
    private readonly object _lockObj = new object();
    public IHostRepository HostRepository;

    public void Run()
    {
            var apartment1 = GetApartmentFromHost(1);
            var apartment2 = GetApartmentFromHost(2);
            SimulateWithoutLock(apartment1);
            SimulateWithLock(apartment2);
    }

    public Apartment GetApartmentFromHost(int id)
    {
        var host = HostRepository.GetHostById(id);
        if (host == null)
        {
            throw new Exception($"Host with id:{id} not found");
        }

        var apartment = host.Apartments.FirstOrDefault();
        if (apartment == null)
        {
            throw new Exception($"Apartment from host with id {id} not found");
        }

        return apartment;
    }

    public async Task SimulateWithoutLock(Apartment apartment)
    {
        var priceBefore = apartment.Price;
        Console.WriteLine($"Apartment info before: {apartment.ToString()}");
        Task task1 = Task.Run(() => IncreasePrice(apartment));
        Task task2 = Task.Run(() => IncreasePrice(apartment));
        await Task.WhenAll(task1, task2);
        Console.WriteLine($"Result simulation without lock objects:");
        Console.WriteLine($"Actual Price: {apartment.Price}");
        Console.WriteLine($"Expected Price: {priceBefore + 1000}");
        Console.WriteLine($"Apartment info after: {apartment.ToString()}");
        Console.WriteLine("=======================================================================");
    }

    public async Task SimulateWithLock(Apartment apartment)
    {
        var priceBefore = apartment.Price;
        Console.WriteLine($"Apartment info before: {apartment.ToString()}");
        Task task1 = Task.Run(() => IncreasePriceWithLock(apartment));
        Task task2 = Task.Run(() => IncreasePriceWithLock(apartment));
        await Task.WhenAll(task1, task2);
        Console.WriteLine($"Result simulation with lock objects:");
        Console.WriteLine($"Actual Price: {apartment.Price}");
        Console.WriteLine($"Expected Price: {priceBefore + 1000}");
        Console.WriteLine($"Apartment info after: {apartment.ToString()}");
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