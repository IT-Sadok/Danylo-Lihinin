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
            apartments = await _apartmentRepository.GetAvailableApartmentsAsync(filter.StartDate.Value,
                filter.EndDate.Value, pageSize, pageNumber);
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

    public async Task<int> UpsertApartmentAsync(UpsertApartmentDto upsertApartmentDto, int currentHostId)
    {
        if (upsertApartmentDto.Id.HasValue)
        {
            var dtoId = upsertApartmentDto.Id.Value;
            var apartmen = await _apartmentRepository.GetApartmentByIdAsync(dtoId);
            if (apartmen is null)
                throw new KeyNotFoundException("Apartment not found");
            if(apartmen.HostId != currentHostId)
                throw new UnauthorizedAccessException("You are not authorized to update the apartment");
        }
        return await _apartmentRepository.UpsertApartmentAsync(upsertApartmentDto, currentHostId);
    }

    public async Task EnsureApartmentExistAsync(int id)
    {
        if (await _apartmentRepository.GetApartmentByIdAsync(id) is null)
            throw new KeyNotFoundException("Apartment with this id doesn't exist");
    }
}