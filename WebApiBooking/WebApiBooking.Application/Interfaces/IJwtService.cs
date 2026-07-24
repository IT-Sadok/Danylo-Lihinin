using WebApiBooking.Domain;

namespace WebApiBooking.Application.Interface;

public interface IJwtService
{
    public string GenerateJwtToken(User user);
}