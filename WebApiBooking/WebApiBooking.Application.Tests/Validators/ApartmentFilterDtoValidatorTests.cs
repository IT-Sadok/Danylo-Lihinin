using FluentValidation.TestHelper;
using WebApiBooking.Application.DTOs;
using WebApiBooking.Application.Validators.Apartments;

namespace WebApiBooking.Application.Tests.Validators;

public class ApartmentFilterDtoValidatorTests
{
    private readonly ApartmentFilterDtoValidator _validator = new();

    private static ApartmentFilterDto ValidDto() => new()
    {
        StartDate = DateTime.UtcNow.Date.AddDays(2),
        EndDate = DateTime.UtcNow.Date.AddDays(10)
    };

    [Fact]
    public void Validate_NoDates_HasNoErrors()
    {
        var dto = ValidDto() with { StartDate = null, EndDate = null };

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_BothDatesValid_HasNoErrors()
    {
        var result = _validator.TestValidate(ValidDto());

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_OnlyStartDateProvided_HasErrors()
    {
        var dto = ValidDto() with { EndDate = null };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrors();
    }

    [Fact]
    public void Validate_OnlyEndDateProvided_HasErrors()
    {
        var dto = ValidDto() with { StartDate = null };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrors();
    }

    [Fact]
    public void Validate_EndDateNotAfterStartDate_HasErrorForEndDate()
    {
        var dto = ValidDto() with { EndDate = ValidDto().StartDate };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.EndDate);
    }

    [Fact]
    public void Validate_StartDateInThePast_HasErrorForStartDate()
    {
        var dto = ValidDto() with { StartDate = DateTime.UtcNow.Date.AddDays(-5) };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.StartDate);
    }

    [Fact]
    public void Validate_StartDateIsToday_HasNoErrorForStartDate()
    {
        var dto = ValidDto() with { StartDate = DateTime.UtcNow.Date, EndDate = DateTime.UtcNow.Date.AddDays(5) };

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveValidationErrorFor(x => x.StartDate);
    }
}