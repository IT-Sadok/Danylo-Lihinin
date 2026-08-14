using Mapster;
using WebApiBooking.Application.DTOs;
using WebApiBooking.Application.Interface;
using WebApiBooking.Application.Interfaces;
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

    public async Task<List<ApartmentResponseDto>> GetApartmentsAsync(ApartmentFilterDto filter)
    {
        var apartments = await _apartmentRepository.GetApartmentsAsync();
        if (filter.EndDate is not null && filter.StartDate is not null)
        {
            var filtredApartments = new List<Apartment>();
            foreach (var apartment in apartments)
            {
                var bookings = await _bookingRepository.GetBookingByApartmentIdAsync(apartment.Id);
                var isAvailable = BookingAvailabilityChecker.IsAvailable(bookings, filter.StartDate.Value, filter.EndDate.Value);
                if (isAvailable)
                    filtredApartments.Add(apartment);
            }
            
            apartments = filtredApartments;
        }

        return apartments.Adapt<List<ApartmentResponseDto>>();
    }

    public async Task EnsureApartmentExistAsync(int id)
    {
        if (await _apartmentRepository.GetApartmentByIdAsync(id) is null)
            throw new KeyNotFoundException("Apartment with this id doesn't exist");
    }
}