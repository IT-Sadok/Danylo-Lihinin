using Mapster;
using WebApiBooking.Application.DTOs;
using WebApiBooking.Application.Interface;
using WebApiBooking.Domain;
using WebApiBooking.Domain.Interfaces;

namespace WebApiBooking.Application.Services;

public class UserService : IUserService
{
    IUserRepository _userRepository;

    public async Task<List<UserDto>> GetUsersAsync()
    {
        var users = await _userRepository.GetUsersAsync();
        return users.Adapt<List<UserDto>>();
    }

    public async Task<UserDto?> GetUserByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        return user.Adapt<UserDto>();
    }

    public async Task UpdateUserAsync(UpdateUserDto userDto, int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user is null)
            throw new KeyNotFoundException();
        userDto.Adapt(user);
        _userRepository.Update(user);
        await _userRepository.SaveChangesAsync();
    }

    public async Task DeleteUserAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user is null)
        {
            throw new KeyNotFoundException();
        }
        _userRepository.Delete(user);
        await _userRepository.SaveChangesAsync();
    }

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
}