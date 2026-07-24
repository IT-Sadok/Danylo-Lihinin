using Mapster;
using WebApiBooking.Application.DTOs;
using WebApiBooking.Application.Interface;
using WebApiBooking.Domain;
using WebApiBooking.Domain.Interfaces;

namespace WebApiBooking.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtService _jwtService;
    
    public async Task<string> LoginAsync(LoginUserDto userDto)
    {
        var user = await _userRepository.GetByEmailAsync(userDto.Email);
        if(user is null || !_passwordHasher.Verify(userDto.Password,user.HashPassword))
            throw new UnauthorizedAccessException("Email or password is incorrect");
        return _jwtService.GenerateJwtToken(user);
    }
    public async Task<string> RegisterAsync(RegisterUserDto userDto)
    {
        var existingUser = await _userRepository.GetByEmailAsync(userDto.Email);
        if(existingUser is not null)
            throw new  UnauthorizedAccessException("Email already exists");
        var user = userDto.Adapt<User>();
        user.HashPassword = _passwordHasher.HashPassword(userDto.Password);
        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();
        return _jwtService.GenerateJwtToken(user);
    }

    public AuthService(IUserRepository userRepository, IPasswordHasher passwordHasher, IJwtService jwtService)
    {
        _userRepository =  userRepository;
        _passwordHasher = passwordHasher;
        _jwtService = jwtService;
    }
}