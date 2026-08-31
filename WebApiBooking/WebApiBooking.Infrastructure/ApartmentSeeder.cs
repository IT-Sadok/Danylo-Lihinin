using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApiBooking.Domain;
using WebApiBooking.Domain.Constants;
using WebApiBooking.Infrastructure.Persistence;

namespace WebApiBooking.Infrastructure;

public static class ApartmentSeeder
{
    public static async Task SeedApartmentAsync(IServiceProvider serviceProvider)
    {
        var dbContext = serviceProvider.GetRequiredService<BookingDbContext>();
        if(await dbContext.Apartments.AnyAsync())
            return;
        
        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

        var user = await userManager.FindByEmailAsync("testhost@gmail.com");
        if (user == null)
        {
            user = new User
            {
                Name = "Test",
                Email = "testhost@gmail.com",
                UserName = "testhost@gmail.com"
            };
            await userManager.CreateAsync(user, "a12345678");
            await userManager.AddToRoleAsync(user, Roles.Host);
        }

        var aparment1 = new Apartment
        {
            Name = "Obolon",
            Rooms = 2,
            Price = 10000,
            HostId = user.Id,
            Host = user
        };
        var aparment2 = new Apartment
        {
            Name = "Darnitsa",
            Rooms = 1,
            Price = 9000,
            HostId = user.Id,
            Host = user
        };
        var aparment3 = new Apartment
        {
            Name = "Svyatoshin",
            Rooms = 3,
            Price = 19000,
            HostId = user.Id,
            Host = user
        };
        
        await dbContext.Apartments.AddRangeAsync(aparment1, aparment2, aparment3);
        await dbContext.SaveChangesAsync();
    }
}