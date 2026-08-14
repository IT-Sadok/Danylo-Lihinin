using FluentValidation;
using WebApiBooking.Application.DTOs;

namespace WebApiBooking.Application.Validators.Bookings;

public class CreateBookingDtoValidator : AbstractValidator<CreateBookingDto>
{
    public CreateBookingDtoValidator()
    {
        RuleFor(b => b.ApartmentId)
            .NotEmpty()
            .Must(id => id > 0);
        RuleFor(b => b)
            .NotEmpty()
            .Must(b => b.EndDate >= b.StartDate);
    }
}