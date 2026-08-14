using WebApiBooking.Application.DTOs;
using WebApiBooking.Domain;

namespace WebApiBooking.Application.Interface;

public interface IApartmentService
{
    public Task<List<ApartmentResponseDto>> GetApartmentsAsync(ApartmentFilterDto filter);
    public Task EnsureApartmentExistAsync(int id);
}