using WebApiBooking.Application.DTOs;
using WebApiBooking.Application.Events;

namespace WebApiBooking.Application.Interfaces;

public interface IUserRepository
{
    public Task UpdateUserProfileAsync(UserProfileUpdated updateEvent);
}