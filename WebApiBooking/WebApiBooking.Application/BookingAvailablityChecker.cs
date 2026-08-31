using WebApiBooking.Domain;

namespace WebApiBooking.Application;

public static class BookingAvailabilityChecker
{
    public static bool IsAvailable(IEnumerable<Booking> existingBookings, DateTime requestedStart, DateTime requestedEnd)
        => existingBookings.All(b => requestedEnd <= b.StartDate || requestedStart >= b.EndDate);
}