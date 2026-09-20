using System.Security;
using System.Text.Json;
using FluentValidation;
using WebApiBooking.Application.DTOs;

namespace WebApiBooking.Application.Validators.Apartments;

public class UpsertApartmentDtoValidator : AbstractValidator<UpsertApartmentDto>
{
    public  UpsertApartmentDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(50);
        RuleFor(x => x.Price)
            .GreaterThan(0);
        RuleFor(x => x.Rooms)
            .GreaterThan((short)0)
            .LessThan((short)10);
        RuleFor(x => x.CustomData)
            .Must(BeValidJsonOrNull)
            .WithMessage("JSON is not valid");
    }

    private bool BeValidJsonOrNull(string? customData)
    {
        if (customData is null)
            return true;
        try
        {
            JsonDocument.Parse(customData);
            return true;
        }
        catch (JsonException)
        {
            return false;
        }
    }
}