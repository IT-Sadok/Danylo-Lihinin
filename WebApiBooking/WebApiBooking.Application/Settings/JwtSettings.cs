using System.ComponentModel.DataAnnotations;

namespace WebApiBooking.Application.Settings;

public class JwtSettings
{
    [Required,MinLength(32)]
    public string SecretKey { get; set; } =  string.Empty;
    [Required]
    public string Issuer { get; set; } = string.Empty;
    [Required]
    public string Audience { get; set; } = string.Empty;
    [Range(1, int.MaxValue)]
    public int ExpirationMinutes { get; set; }
}