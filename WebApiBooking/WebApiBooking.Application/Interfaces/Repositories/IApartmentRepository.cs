using WebApiBooking.Application.Models;
using WebApiBooking.Domain;

namespace WebApiBooking.Application.Interfaces;

public interface IApartmentRepository
{
    public Task<PagedResult<ApartmentWithHost>> GetApartmentsAsync(int pageSize, int pageNumber);
    public Task<Apartment?> GetApartmentByIdAsync(int id);
    public Task<PagedResult<ApartmentWithHost>> GetAvailableApartmentsAsync(DateTime startDate, DateTime endDate,int pageSize, int pageNumber);
}