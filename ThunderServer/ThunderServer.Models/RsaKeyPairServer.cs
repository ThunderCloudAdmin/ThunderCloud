using Shared.Models;

namespace ThunderServer.Models;

public class RsaKeyPairServer : RsaKeyPair
{
    public RsaKeyPairServer(string publicKey, string privateKey) : base(publicKey, privateKey) { }

    public byte[] Salt { get; set; }

    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    //Navigations
    public ThunderUser User { get; set; }
}