using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebApiBooking.Application.Settings;
using WebApiBooking.Domain;
using Shouldly;

namespace WebApiBooking.Infrastructure.Tests;

public class JwtServiceTests
{
    private const string SecretKey = "super-secret-test-key-minimum-32-characters!";
    private const string Issuer = "TestIssuer";
    private const string Audience = "TestAudience";
    private const int ExpirationMinutes = 60;

    private readonly JwtService _service;
    private readonly JwtSettings _jwtSettings;

    public JwtServiceTests()
    {
        _jwtSettings = new JwtSettings
        {
            SecretKey = SecretKey,
            Issuer = Issuer,
            Audience = Audience,
            ExpirationMinutes = ExpirationMinutes
        };

        _service = new JwtService(Options.Create(_jwtSettings));
    }

    private static User CreateUser(int id = 1, string email = "test@test.com", string name = "TestUser")
    {
        return new User
        {
            Id = id,
            Email = email,
            Name = name
        };
    }

    [Fact]
    public void GenerateJwtToken_ShouldReturnNonEmptyToken()
    {
        var user = CreateUser();
        
        var token = _service.GenerateJwtToken(user, new List<string> { "Client" });
        
        token.ShouldNotBeNullOrWhiteSpace();
    }

    [Fact]
    public void GenerateJwtToken_ShouldContainCorrectUserClaims()
    {
        var user = CreateUser(id: 42, email: "danylo@test.com", name: "Danylo");
        
        var token = _service.GenerateJwtToken(user, new List<string> { "Host" });
        var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
        
        jwtToken.Claims.First(c => c.Type == JwtRegisteredClaimNames.Sub).Value.ShouldBe("42");
        jwtToken.Claims.First(c => c.Type == JwtRegisteredClaimNames.Email).Value.ShouldBe("danylo@test.com");
        jwtToken.Claims.First(c => c.Type == ClaimTypes.Name).Value.ShouldBe("Danylo");
    }

    [Fact]
    public void GenerateJwtToken_ShouldContainAllProvidedRoles()
    {
        var user = CreateUser();
        var roles = new List<string> { "Client", "Host" };

        var token = _service.GenerateJwtToken(user, roles);
        var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);

        var roleClaims = jwtToken.Claims
            .Where(c => c.Type == ClaimTypes.Role)
            .Select(c => c.Value)
            .ToList();

        roleClaims.ShouldBe(roles, ignoreOrder: true);
    }

    [Fact]
    public void GenerateJwtToken_ShouldNotContainRoleClaims_WhenRolesListIsEmpty()
    {
        var user = CreateUser();

        var token = _service.GenerateJwtToken(user, new List<string>());
        var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);

        jwtToken.Claims.Any(c => c.Type == ClaimTypes.Role).ShouldBeFalse();
    }

    [Fact]
    public void GenerateJwtToken_ShouldSetCorrectIssuerAndAudience()
    {
        var user = CreateUser();

        var token = _service.GenerateJwtToken(user, new List<string>());
        var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);

        jwtToken.Issuer.ShouldBe(Issuer);
        jwtToken.Audiences.ShouldContain(Audience);
    }

    [Fact]
    public void GenerateJwtToken_ShouldSetExpirationBasedOnConfiguredMinutes()
    {
        var user = CreateUser();
        var before = DateTime.UtcNow;

        var token = _service.GenerateJwtToken(user, new List<string>());
        var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
        var after = DateTime.UtcNow;

        var expectedMin = before.AddMinutes(ExpirationMinutes);
        var expectedMax = after.AddMinutes(ExpirationMinutes);

        jwtToken.ValidTo.ShouldBeInRange(expectedMin.AddSeconds(-5), expectedMax.AddSeconds(5));
    }

    [Fact]
    public void GenerateJwtToken_ShouldProduceTokenThatPassesFullValidation()
    {
        var user = CreateUser(id: 7, email: "valid@test.com", name: "ValidUser");
        var token = _service.GenerateJwtToken(user, new List<string> { "Client" });

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = Issuer,
            ValidateAudience = true,
            ValidAudience = Audience,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey))
        };

        Action validate = () => new JwtSecurityTokenHandler()
            .ValidateToken(token, validationParameters, out _);

        Should.NotThrow(validate);
    }

    [Fact]
    public void GenerateJwtToken_ShouldUseHmacSha256SigningAlgorithm()
    {
        var user = CreateUser();

        var token = _service.GenerateJwtToken(user, new List<string>());
        var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);

        jwtToken.SignatureAlgorithm.ShouldBe(SecurityAlgorithms.HmacSha256);
    }
}