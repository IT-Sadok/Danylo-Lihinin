using FluentValidation.TestHelper;
using WebApiBooking.Application.DTOs;
using WebApiBooking.Application.Validators;
using WebApiBooking.Domain.Constants;

namespace WebApiBooking.Application.Tests.Validators;

public class RegisterUserDtoValidatorsTest
{
    private readonly RegisterUserDtoValidator _validator = new();

    private static RegisterUserDto ValidDto() => new()
    {
        Name = "Danylo",
        Email = "danylo@gmail.com",
        Password = "123456",
        Role = Roles.Client
    };

    [Fact]
    public void Validate_ValidDto_HasNoErrors()
    {
        var result = _validator.TestValidate(ValidDto());
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_EmptyName_HasErrorForName()
    {
        var dto = ValidDto() with { Name = "" };
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

    [Theory]
    [InlineData("")]
    [InlineData("12345")] 
    public void Validate_InvalidPassword_HasErrorForPassword(string password)
    {
        var dto = ValidDto() with { Password = password };
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Validate_AdminRole_HasErrorForRole()
    {
        var dto = ValidDto() with { Role = Roles.Admin };
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.Role);
    }

    [Theory]
    [InlineData(Roles.Client)]
    [InlineData(Roles.Host)]
    public void Validate_AllowedRole_HasNoErrorForRole(string role)
    {
        var dto = ValidDto() with { Role = role };
        var result = _validator.TestValidate(dto);
        result.ShouldNotHaveValidationErrorFor(x => x.Role);
    }

    [Fact]
    public void Validate_UnknownRole_HasErrorForRole()
    {
        var dto = ValidDto() with { Role = "SuperAdmin" };
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.Role);
    }
}
