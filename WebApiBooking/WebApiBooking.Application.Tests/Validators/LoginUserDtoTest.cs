using FluentValidation.TestHelper;
using WebApiBooking.Application.DTOs;
using WebApiBooking.Application.Validators;

namespace WebApiBooking.Application.Tests.Validators;

public class LoginUserDtoTest
{
    private readonly LoginUserDtoValidator _validator = new();

    private static LoginUserDto ValidDto() => new()
    {
        Email = "danylo@gmail.com",
        Password = "123456",
    };
    
    [Fact]
    public void Validate_ValidDto_HasNoErrors()
    {
        var result = _validator.TestValidate(ValidDto());
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData("")]
    [InlineData("danylogmail.com")]
    public void Validate_InvalidEmail_HasErrorForEmail(string email)
    {
        var dto =  ValidDto() with { Email = email };
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData("12345")] 
    public void Validate_InvalidPassword_HasErrorForPassword(string password)
    {
        var dto = ValidDto() with { Password = password };
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }
}