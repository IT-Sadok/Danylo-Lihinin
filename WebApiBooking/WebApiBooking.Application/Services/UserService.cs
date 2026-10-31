using WebApiBooking.Application.DTOs;
using WebApiBooking.Application.Events;
using WebApiBooking.Application.Interface;
using WebApiBooking.Application.Interfaces;

namespace WebApiBooking.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    
    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task UpdateProfileAsync(UpdateUserDto updateUserDto, int userId)
    {
        var snapshot = new ProfileSnapshot
        {
            Name = updateUserDto.Name,
            Email = updateUserDto.Email,
        };
        var updateEvent = new UserProfileUpdated
        {
            EventId = Guid.NewGuid(),
            SchemaVersion = 1,
            UserId = userId,
            OccurredAt = DateTimeOffset.UtcNow,
            Snapshot = snapshot
        };
            
        await _userRepository.UpdateUserProfileAsync(updateEvent);
    }
}