using Microsoft.AspNetCore.Identity;

namespace ThunderServer.Models;

public class ThunderUser : IdentityUser<Guid>
{
    public string FirstName { get; init; }

    public string LastName { get; init; }

    public Guid RsaKeyPairServerId { get; set; }

    //Navigation
    public RsaKeyPairServer RsaKeyPairServer { get; set; }
}
