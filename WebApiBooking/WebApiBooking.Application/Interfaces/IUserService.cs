using WebApiBooking.Application.DTOs;
using WebApiBooking.Domain;

namespace WebApiBooking.Application.Interface;

public interface IUserService
{
    public Task<List<UserDto>> GetUsersAsync();
    public Task<UserDto?> GetUserByIdAsync(int id);
    public Task UpdateUserAsync(UpdateUserDto userDto, int id);
    public Task DeleteUserAsync(int id);
}