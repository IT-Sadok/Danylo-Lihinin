using FluentValidation.TestHelper;
using WebApiBooking.Application.DTOs;
using WebApiBooking.Application.Validators.Bookings;

namespace WebApiBooking.Application.Tests.Validators;

public class CreateBookingDtoValidatorTests
{
    private readonly CreateBookingDtoValidator _validator = new();

    private static CreateBookingDto validDto() => new()
    {
        ApartmentId = 1,
        StartDate = DateTime.Now.AddDays(2),
        EndDate = DateTime.Now.AddDays(10),
    };

    [Fact]

    public void Validate_ValidDto_HasNoErrors()
    {
        var result = _validator.TestValidate(validDto());
        result.ShouldNotHaveAnyValidationErrors();
    }
    
    [Theory]
    [InlineData(-2)]
    [InlineData(0)]

    public void Validate_InvalidId_HasErrorsForId(int id)
    {
        var dto = validDto() with {ApartmentId =  id};
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.ApartmentId);
    }

    [Fact]
    public void Validate_StartDateBiggerThanEndDate_HasErrors()
    {
        var dto = validDto() with { EndDate = DateTime.Now.AddDays(-1) };
        
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrors();
    }

    [Fact]
    public void Validate_StartDateInThePast_HasErrors()
    {
        var dto  = validDto() with { StartDate = DateTime.Now.AddDays(-1) };
        var result = _validator.TestValidate(dto);
        
        result.ShouldHaveValidationErrorFor(x => x.StartDate);
    }
}