using FluentValidation.TestHelper;
using WebApiBooking.Application.DTOs;
using WebApiBooking.Application.Validators;
using WebApiBooking.Domain.Constants;

namespace WebApiBooking.Application.Tests.Validators;

public class UpdateUserDtoTest
{
    private readonly UpdateUserDtoValidator _validator = new ();
    
    private static UpdateUserDto ValidDto() => new()
    {
        Name = "Danylo",
        Email = "danylo@gmail.com",
    };
    
    [Fact]
    public void Validate_ValidDto_HasNoErrors()
    {
        var result = _validator.TestValidate(ValidDto());
        result.ShouldNotHaveAnyValidationErrors();
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("tyhjgkjhgbnmkiuytgbnkiuytfvbnjkiuytghjkiuytghnjkiuytghnjkiuytretyuioliuytre")]
    public void Validate_EmptyName_HasErrorForName(string name)
    {
        var dto = ValidDto() with { Name = name };
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData("not-an-email")]
    public void Validate_InvalidEmail_HasErrorForEmail(string email)
    {
        var dto = ValidDto() with { Email = email };
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }
}