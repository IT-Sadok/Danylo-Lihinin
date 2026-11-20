using WebApiBooking.Application.DTOs;

namespace WebApiBooking.Application.Interface;

public interface IUserService
{
    public Task UpdateProfileAsync(UpdateUserDto updateUserDto, int userId);
}