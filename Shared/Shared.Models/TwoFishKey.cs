namespace Shared.Models;

public class TwoFishKey : BaseEncryptionKey
{
    public TwoFishKey(byte[] key, byte[] iv) : base(key, iv) { }
}
