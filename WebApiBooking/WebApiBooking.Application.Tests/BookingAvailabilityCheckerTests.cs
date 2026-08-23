using Shouldly;
using WebApiBooking.Domain;

namespace WebApiBooking.Application.Tests;

public class BookingAvailabilityCheckerTests
{
    [Theory]
    [InlineData("2026-06-01", "2026-06-10", "2026-06-15", "2026-06-20", true)]
    [InlineData("2026-06-15", "2026-06-20", "2026-06-01", "2026-06-10", true)]
    [InlineData("2026-06-05", "2026-06-15", "2026-06-01", "2026-06-10", false)]
    [InlineData("2026-06-01", "2026-06-10", "2026-06-10", "2026-06-15", true)]

    public void IsAvailable_SingleBooking_ReturnsExpectedResult(string existingStart, string existingEnd, string requestedStart, string requestedEnd, bool expected)
    {
        var bookings = new List<Booking>
        {
            new Booking
            {
                StartDate = DateTime.Parse(existingStart),
                EndDate = DateTime.Parse(existingEnd),
            }
        };
        
        var result = BookingAvailabilityChecker.IsAvailable(bookings, DateTime.Parse(requestedStart), DateTime.Parse(requestedEnd));
        result.ShouldBe(expected);
    }
    
    [Fact]
    public void IsAvailable_NoExistingBookings_ReturnsTrue()
    {
        var bookings = new List<Booking>();
        
        var result = BookingAvailabilityChecker.IsAvailable(bookings, DateTime.Parse("2026-06-01"), DateTime.Parse("2026-06-10"));

        result.ShouldBeTrue();
    }
    
    [Fact]
    public void IsAvailable_MultipleBookings_OneOverlaps_ReturnsFalse()
    {
        var bookings = new List<Booking>
        {
            new Booking { StartDate = DateTime.Parse("2026-06-01"), EndDate = DateTime.Parse("2026-06-05") },
            new Booking { StartDate = DateTime.Parse("2026-06-15"), EndDate = DateTime.Parse("2026-06-20") }
        };

        var result = BookingAvailabilityChecker.IsAvailable(bookings, DateTime.Parse("2026-06-03"),DateTime.Parse("2026-06-08"));

        result.ShouldBeFalse();
    }
    
    [Fact]
    public void IsAvailable_MultipleBookings_NoneOverlap_ReturnsTrue()
    {
        var bookings = new List<Booking>
        {
            new Booking { StartDate = DateTime.Parse("2026-06-01"), EndDate = DateTime.Parse("2026-06-05") },
            new Booking { StartDate = DateTime.Parse("2026-06-15"), EndDate = DateTime.Parse("2026-06-20") }
        };

        var result = BookingAvailabilityChecker.IsAvailable(bookings, DateTime.Parse("2026-06-08"), DateTime.Parse("2026-06-12"));

        result.ShouldBeTrue();
    }
}