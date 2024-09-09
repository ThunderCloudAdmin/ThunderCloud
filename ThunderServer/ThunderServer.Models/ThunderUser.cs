using Microsoft.AspNetCore.Identity;

namespace ThunderServer.Models;

public class ThunderUser : IdentityUser
{
    public string FirstName { get; set; }

    public string LastName { get; set; }
}
