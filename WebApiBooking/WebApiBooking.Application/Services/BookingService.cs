using Mapster;
using WebApiBooking.Application.DTOs;
using WebApiBooking.Application.Interface;
using WebApiBooking.Application.Interfaces;
using WebApiBooking.Domain;

namespace WebApiBooking.Application.Services;

public class BookingService : IBookingService
{
    private IBookingRepository _bookingRepository;
    private IApartmentService _apartmentService;

    public BookingService(IBookingRepository bookingRepository, IApartmentService apartmentService)
    {
        _bookingRepository = bookingRepository;
        _apartmentService = apartmentService;
    }

    public async Task CreateBookingAsync(CreateBookingDto dto, int userId)
    {
        await _apartmentService.EnsureApartmentExistAsync(dto.ApartmentId);
        if (!await IsApartmentAvailableAsync(dto.ApartmentId, dto.StartDate, dto.EndDate))
            throw new InvalidOperationException("Apartment is not available for the selected dates");
        var booking = dto.Adapt<Booking>();
        booking.UserId = userId;
        await _bookingRepository.CreateBookingAsync(booking);
    }

    public async Task<List<BookingResponseDto>> GetBookingsAsync(int userId)
    {
        var bookings = await _bookingRepository.GetBookingByUserIdAsync(userId);
        return bookings.Select(b => b.Adapt<BookingResponseDto>()).ToList();
    }

    public async Task<bool> IsApartmentAvailableAsync(int apartmentId, DateTime startDate, DateTime endDate)
    {
        return await _bookingRepository.IsApartmentAvailableAsync(apartmentId, startDate, endDate);
    }
}