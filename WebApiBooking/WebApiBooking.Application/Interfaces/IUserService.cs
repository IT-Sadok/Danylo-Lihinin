using WebApiBooking.Domain;

namespace WebApiBooking.Application.Interface;

public interface IUserService
{
    public Task AddUserAsync(User user);
    public Task<List<User>> GetUsersAsync();
    public Task<User?> GetUserByIdAsync(int id);
    public Task UpdateUserAsync(User user);
    public Task DeleteUserAsync(int id);
}