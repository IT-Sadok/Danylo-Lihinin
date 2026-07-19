namespace WebApiBooking.Domain.Interfaces;

public interface IUserRepository
{
    public Task AddAsync(User user);
    public Task<User?> GetByIdAsync(int id);
    public Task<List<User>> GetUsersAsync();
    public void Update(User user);
    public void Delete(User user);
    Task SaveChangesAsync();
}