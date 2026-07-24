using WebApiBooking.Application.Interface;

namespace WebApiBooking.Infrastructure;

public class BCryptPasswordHasher : IPasswordHasher
{
    public string HashPassword(string password)
    {
       return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool Verify(string password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
}