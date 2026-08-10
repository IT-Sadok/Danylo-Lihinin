using FluentValidation;
using WebApiBooking.Application.DTOs;
using WebApiBooking.Domain.Constants;

namespace WebApiBooking.Application.Validators;

public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
{
    public RegisterUserDtoValidator()
    {
        RuleFor(u => u.Name)
            .NotEmpty()
            .MaximumLength(50);
        RuleFor(u => u.Email)
            .NotEmpty()
            .EmailAddress();
        RuleFor(u => u.Password)
            .NotEmpty()
            .MinimumLength(6);
        RuleFor(u => u.Role)
            .NotEmpty()
            .Must(role => role == Roles.Client || role == Roles.Host);
    }
}