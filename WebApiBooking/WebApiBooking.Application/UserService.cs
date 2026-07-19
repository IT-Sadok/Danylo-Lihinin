using WebApiBooking.Application.Interface;
using WebApiBooking.Domain;
using WebApiBooking.Domain.Interfaces;

namespace WebApiBooking.Application;

public class UserService : IUserService
{
    IUserRepository _userRepository;

    public async Task AddUserAsync(User user)
    {
        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();
    }

    public Task<List<User>> GetUsersAsync()
    {
        return  _userRepository.GetUsersAsync();
    }

    public Task<User> GetUserByIdAsync(int id)
    {
        return _userRepository.GetByIdAsync(id);
    }

    public async Task UpdateUserAsync(User user)
    {
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