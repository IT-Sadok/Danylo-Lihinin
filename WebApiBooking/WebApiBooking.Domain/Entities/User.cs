using Microsoft.AspNetCore.Identity;

namespace WebApiBooking.Domain;

public class User : IdentityUser<int>
{
    public string Name { get; set; }
}
