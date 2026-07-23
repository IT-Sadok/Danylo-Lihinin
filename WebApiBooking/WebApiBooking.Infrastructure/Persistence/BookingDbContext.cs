using Microsoft.EntityFrameworkCore;
using WebApiBooking.Domain;

namespace WebApiBooking.Infrastructure.Persistence;

public class BookingDbContext : DbContext
{
    public DbSet<User> Users =>  Set<User>();
    public BookingDbContext(DbContextOptions<BookingDbContext> options) : base(options)
    {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .Property(u => u.Role)
            .HasConversion<string>()
            .HasMaxLength(20);
    }
}