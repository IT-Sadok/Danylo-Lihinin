using WebApiBooking.Domain;

namespace WebApiBooking.Application.Interfaces;

public interface IApartmentRepository
{
    public Task<List<Apartment>> GetApartmentsAsync();
    public Task<Apartment?> GetApartmentByIdAsync(int id);
}