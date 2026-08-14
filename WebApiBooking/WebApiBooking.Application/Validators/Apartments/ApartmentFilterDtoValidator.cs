using FluentValidation;
using WebApiBooking.Application.DTOs;

namespace WebApiBooking.Application.Validators.Apartments;

public class ApartmentFilterDtoValidator : AbstractValidator<ApartmentFilterDto>
{
    public ApartmentFilterDtoValidator()
    {
        RuleFor(x => x)
            .Must(f => f.StartDate.HasValue == f.EndDate.HasValue)
            .WithMessage("Both StartDate and EndDate must be provided together, or neither.");

        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate)
            .When(x => x.StartDate.HasValue && x.EndDate.HasValue);
        
        RuleFor(x => x.StartDate)
            .GreaterThanOrEqualTo(DateTime.UtcNow.Date)
            .When(x => x.StartDate.HasValue);
    }
}