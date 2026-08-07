using Microsoft.AspNetCore.Identity;
using Moq;
using Shouldly;
using WebApiBooking.Application.DTOs;
using WebApiBooking.Application.Interface;
using WebApiBooking.Application.Services;
using WebApiBooking.Domain;

namespace WebApiBooking.Application.Tests.Services;

public class AuthServiceTests
{
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly Mock<IJwtService> _jwtServiceMock;
    private readonly AuthService _authService;
    
    public AuthServiceTests()
    {
        var storeMock = new Mock<IUserStore<User>>();
        _userManagerMock = new Mock<UserManager<User>>(storeMock.Object, null, null, null, null, null, null, null, null);
        _jwtServiceMock = new Mock<IJwtService>();
        _authService = new AuthService(_userManagerMock.Object, _jwtServiceMock.Object);
    }

    [Fact]
    public async Task RegisterAsync_EmailAlreadyExists_ThrowsUnAuthorizedException()
    {
        var dto = new LoginUserDto{Email = "user@gmail.com",  Password = "123456"};
        _userManagerMock
            .Setup(x => x.FindByEmailAsync(dto.Email))
            .ReturnsAsync((User?)null);
        
        await Should.ThrowAsync<UnauthorizedAccessException>(() => _authService.LoginAsync(dto));
    }
    [Fact]
    public async Task RegisterAsync_CreateAsyncFails_ThrowsWithErrorMessages()
    {
        var dto = new RegisterUserDto
        {
            Name = "Danylo", Email = "danylo@gmail.com", Password = "123456", Role = "Client"
        };
        _userManagerMock
            .Setup(x => x.FindByEmailAsync(dto.Email))
            .ReturnsAsync((User?)null);
        _userManagerMock
            .Setup(x => x.CreateAsync(It.IsAny<User>(), dto.Password))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Weak password" }));

        var exception = await Should.ThrowAsync<UnauthorizedAccessException>(
            () => _authService.RegisterAsync(dto));
        
        exception.Message.ShouldContain("Weak password");
    }
    [Fact]
    public async Task RegisterAsync_ValidDto_AddsRoleAndReturnsToken()
    {
        var dto = new RegisterUserDto
        {
            Name = "Danylo", Email = "danylo@gmail.com", Password = "123456", Role = "Client"
        };

        _userManagerMock.Setup(x => x.FindByEmailAsync(dto.Email))
            .ReturnsAsync((User?)null);
        _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<User>(), dto.Password))
            .ReturnsAsync(IdentityResult.Success);
        _userManagerMock.Setup(x => x.AddToRoleAsync(It.IsAny<User>(), dto.Role))
            .ReturnsAsync(IdentityResult.Success);
        _userManagerMock.Setup(x => x.GetRolesAsync(It.IsAny<User>()))
            .ReturnsAsync(new List<string> { dto.Role });
        _jwtServiceMock.Setup(x => x.GenerateJwtToken(It.IsAny<User>(), It.IsAny<IList<string>>()))
            .Returns("fake-jwt-token");
        
        var result = await _authService.RegisterAsync(dto);
        
        result.ShouldBe("fake-jwt-token");
        _userManagerMock.Verify(
            x => x.AddToRoleAsync(It.IsAny<User>(), dto.Role),
            Times.Once);
    }

    [Fact]
    public async Task LoginAsync_UserNotFound_ThrowsUnauthorizedAccessException()
    {
        var dto = new LoginUserDto { Email = "test@test.com", Password = "123456" };
        _userManagerMock
            .Setup(x => x.FindByEmailAsync(dto.Email))
            .ReturnsAsync((User?)null);
        
        await Should.ThrowAsync<UnauthorizedAccessException>(
            () => _authService.LoginAsync(dto));
    }

    [Fact]
    public async Task LoginAsync_WrongPassword_ThrowsUnAuthorizedException()
    {
        var dto = new LoginUserDto{Email = "user@gmail.com",  Password = "wrong"};
        var user = new User { Email = dto.Email };
        
        _userManagerMock.Setup(x => x.FindByEmailAsync(dto.Email)).ReturnsAsync(user);
        _userManagerMock.Setup(x => x.CheckPasswordAsync(user, dto.Password)).ReturnsAsync(false);
        await Should.ThrowAsync<UnauthorizedAccessException>(() => _authService.LoginAsync(dto));
    }
    [Fact]
    public async Task LoginAsync_ValidCredentials_ReturnsToken()
    {
        var dto = new LoginUserDto { Email = "test@test.com", Password = "123456" };
        var user = new User { Email = dto.Email };

        _userManagerMock.Setup(x => x.FindByEmailAsync(dto.Email)).ReturnsAsync(user);
        _userManagerMock.Setup(x => x.CheckPasswordAsync(user, dto.Password)).ReturnsAsync(true);
        _userManagerMock.Setup(x => x.GetRolesAsync(user)).ReturnsAsync(new List<string> { "Client" });
        _jwtServiceMock.Setup(x => x.GenerateJwtToken(user, It.IsAny<IList<string>>()))
            .Returns("fake-jwt-token");
        
        var result = await _authService.LoginAsync(dto);
        
        result.ShouldBe("fake-jwt-token");
    }
}