using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApiBooking.Domain;

namespace WebApiBooking.Infrastructure.Persistence;

public class BookingDbContext : IdentityDbContext<User, IdentityRole<int>,int>
{
    public DbSet<User> Users =>  Set<User>();
    public BookingDbContext(DbContextOptions<BookingDbContext> options) : base(options)
    {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}