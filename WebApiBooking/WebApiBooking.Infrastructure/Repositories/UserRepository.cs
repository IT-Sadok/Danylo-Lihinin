using System.IO.Pipelines;
using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApiBooking.Application.Events;
using WebApiBooking.Application.Exceptions;
using WebApiBooking.Application.Interfaces;
using WebApiBooking.Domain;
using WebApiBooking.Infrastructure.Persistence;

namespace WebApiBooking.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    BookingDbContext _dbContext;
    UserManager<User> _userManager;
    
    public UserRepository(BookingDbContext dbContext, UserManager<User> userManager)
    {
        _dbContext = dbContext;
        _userManager = userManager;
    }
    public async Task UpdateUserProfileAsync(UserProfileUpdated updateEvent)
    {
        await using var tx = await _dbContext.Database.BeginTransactionAsync();
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == updateEvent.UserId);
        if (user == null)
            throw new KeyNotFoundException("Id is not found");
        if (user.Name == updateEvent.Snapshot.Name && user.Email == updateEvent.Snapshot.Email)
            return;
        user.ProfileVersion++;
        user.Name = updateEvent.Snapshot.Name;
        user.Email = updateEvent.Snapshot.Email;
        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            if(result.Errors.Any(e => e.Code == nameof(IdentityErrorDescriber.ConcurrencyFailure)))
                throw new ConflictException("Concurrency failure");
            if(result.Errors.Any(e => e.Code == nameof(IdentityErrorDescriber.DuplicateEmail)))
                throw new ConflictException("Duplicate email");
            throw new InvalidOperationException(string.Join("; ", result.Errors.Select(e => e.Description)));
        }
        var versioned = updateEvent with { Version = user.ProfileVersion };
        var jsonEvent =  JsonSerializer.Serialize(versioned);
        var outboxMessage = new OutboxMessage
        {
            Id = versioned.EventId,
            Type = nameof(UserProfileUpdated),
            CreatedAt = DateTimeOffset.UtcNow,
            Payload = jsonEvent,
            ProcessedAt = null
        };
        await _dbContext.OutboxMessages.AddAsync(outboxMessage);
        await _dbContext.SaveChangesAsync();
        await tx.CommitAsync();
    }
}