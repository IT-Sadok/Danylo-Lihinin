using Mapster;
using WebApiBooking.Application.DTOs;
using WebApiBooking.Application.Interface;
using WebApiBooking.Application.Interfaces;
using WebApiBooking.Application.Models;
using WebApiBooking.Domain;

namespace WebApiBooking.Application.Services;

public class ApartmentService : IApartmentService
{
    private IApartmentRepository _apartmentRepository;
    private IBookingRepository _bookingRepository;

    public ApartmentService(IApartmentRepository apartmentRepository, IBookingRepository bookingRepository)
    {
        _apartmentRepository = apartmentRepository;
        _bookingRepository = bookingRepository;
    }

    public async Task<PagedResult<ApartmentResponseDto>> GetApartmentsAsync(ApartmentFilterDto filter)
    {
        PagedResult<ApartmentWithHost> apartments;
        var pageSize = filter.PageSize ?? 10;
        var pageNumber = filter.PageNumber ?? 1;
        
        if (filter.EndDate is not null && filter.StartDate is not null)
        {
            apartments = await _apartmentRepository.GetAvailableApartmentsAsync(filter.StartDate.Value, filter.EndDate.Value, pageSize, pageNumber);
        }
        else
        {
            apartments = await _apartmentRepository.GetApartmentsAsync(pageSize, pageNumber);
        }

        var response = new PagedResult<ApartmentResponseDto>();
        response.TotalCount = apartments.TotalCount;
        response.Items = apartments.Items.Adapt<List<ApartmentResponseDto>>();
        return response;
    }

    public async Task EnsureApartmentExistAsync(int id)
    {
        if (await _apartmentRepository.GetApartmentByIdAsync(id) is null)
            throw new KeyNotFoundException("Apartment with this id doesn't exist");
    }
}