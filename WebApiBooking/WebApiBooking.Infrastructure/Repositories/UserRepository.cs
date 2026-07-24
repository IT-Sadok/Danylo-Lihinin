using Microsoft.EntityFrameworkCore;
using WebApiBooking.Domain;
using WebApiBooking.Domain.Interfaces;
using WebApiBooking.Infrastructure.Persistence;

namespace WebApiBooking.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private BookingDbContext _context;

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
    }
    public async  Task<List<User>> GetUsersAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public void Update(User user)
    {
        _context.Users.Update(user);
    }
    public void Delete(User user)
    {
        _context.Users.Remove(user);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public UserRepository(BookingDbContext context)
    {
        _context = context;
    }
}