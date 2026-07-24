using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WebApiBooking.Application.Interface;
using WebApiBooking.Domain;

namespace WebApiBooking.Infrastructure;

public class JwtService : IJwtService
{
    private readonly IConfiguration _configuration;
    public string GenerateJwtToken(User user)
    {
        var jwtSection =  _configuration.GetSection("Jwt");
        var secretKey = jwtSection["SecretKey"];

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(ClaimTypes.Name, user.Name),
            new(ClaimTypes.Role, user.Role.ToString()),
        };
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        var expirationMinutes = int.Parse(jwtSection["ExpirationMinutes"]);
        var token = new JwtSecurityToken(
            issuer: jwtSection["Issuer"],
            audience: jwtSection["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
            signingCredentials: creds);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
}