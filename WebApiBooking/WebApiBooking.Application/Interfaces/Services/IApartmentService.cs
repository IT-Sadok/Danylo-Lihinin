using WebApiBooking.Application.DTOs;
using WebApiBooking.Application.Models;
using WebApiBooking.Domain;

namespace WebApiBooking.Application.Interface;

public interface IApartmentService
{
    public Task<PagedResult<ApartmentResponseDto>> GetApartmentsAsync(ApartmentFilterDto filter);
    public Task EnsureApartmentExistAsync(int id);
}