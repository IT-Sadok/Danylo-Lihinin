using FluentValidation;
using WebApiBooking.Application.DTOs;

namespace WebApiBooking.Application.Validators;

public class LoginUserDtoValidator : AbstractValidator<LoginUserDto>
{
    public LoginUserDtoValidator()
    {
        RuleFor(u => u.Email)
            .NotEmpty()
            .EmailAddress();
        RuleFor(u => u.Password)
            .NotEmpty()
            .MinimumLength(6);
    }
}