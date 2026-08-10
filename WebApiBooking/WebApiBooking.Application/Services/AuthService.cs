using Mapster;
using Microsoft.AspNetCore.Identity;
using WebApiBooking.Application.DTOs;
using WebApiBooking.Application.Interface;
using WebApiBooking.Domain;

namespace WebApiBooking.Application.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly IJwtService _jwtService;

    public AuthService(UserManager<User> userManager, IJwtService jwtService)
    {
        _userManager = userManager;
        _jwtService = jwtService;
    }

    public async Task<string> LoginAsync(LoginUserDto userDto)
    {
        var user = await _userManager.FindByEmailAsync(userDto.Email);
        if (user is null || !await _userManager.CheckPasswordAsync(user, userDto.Password))
            throw new UnauthorizedAccessException("Invalid login attempt");
        
        var roles = await _userManager.GetRolesAsync(user);
        return _jwtService.GenerateJwtToken(user, roles);
    }

    public async Task<string> RegisterAsync(RegisterUserDto userDto)
    {
        var existingUser = await _userManager.FindByEmailAsync(userDto.Email);

        if (existingUser is not null)
            throw new UnauthorizedAccessException("Email already exists");

        var user = userDto.Adapt<User>();
        user.UserName = userDto.Email;
        
        var result = await _userManager.CreateAsync(user, userDto.Password);
        if (!result.Succeeded)
        {
            var errors = string.Join(";",result.Errors.Select(x => x.Description));
            throw new UnauthorizedAccessException(errors);
        }
        
        await _userManager.AddToRoleAsync(user,userDto.Role);
        var roles = await _userManager.GetRolesAsync(user);
        return _jwtService.GenerateJwtToken(user, roles);
    }
}