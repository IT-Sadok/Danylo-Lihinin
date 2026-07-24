namespace WebApiBooking.Application.Interface;

public interface IPasswordHasher
{
    string HashPassword(string password);
    bool Verify(string password, string hashPassword);
}