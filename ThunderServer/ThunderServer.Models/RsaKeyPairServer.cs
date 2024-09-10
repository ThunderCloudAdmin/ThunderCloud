using Shared.Models;

namespace ThunderServer.Models;

public class RsaKeyPairServer : RsaKeyPair
{
    protected RsaKeyPairServer() : base() { }

    public byte[] EncryptedPrivateRsaKey { get; set; }

    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    //Navigations
    public ThunderUser User { get; set; }

    public Argon2Key Argon2Key { get; set; }
}