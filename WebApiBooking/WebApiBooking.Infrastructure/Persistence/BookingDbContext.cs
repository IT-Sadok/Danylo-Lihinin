using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApiBooking.Domain;

namespace WebApiBooking.Infrastructure.Persistence;

public class BookingDbContext : IdentityDbContext<User, IdentityRole<int>,int>
{
    public DbSet<User> Users =>  Set<User>();
    public DbSet<Apartment> Apartments =>  Set<Apartment>();
    public DbSet<Booking> Bookings =>  Set<Booking>();
    public BookingDbContext(DbContextOptions<BookingDbContext> options) : base(options)
    {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<User>()
            .HasIndex(u => u.ExternalId)
            .IsUnique();
        modelBuilder.Entity<Apartment>()
            .HasIndex(a => a.ExternalId)
            .IsUnique();
        modelBuilder.Entity<Apartment>()
            .HasOne(a => a.Host)
            .WithMany(u => u.OwnedApartments)
            .HasForeignKey(a => a.HostId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Booking>()
            .HasOne(b => b.User)
            .WithMany(u => u.Bookings)
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Booking>()
            .HasOne(b => b.Apartment)
            .WithMany(u => u.Bookings)
            .HasForeignKey(b => b.ApartmentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}