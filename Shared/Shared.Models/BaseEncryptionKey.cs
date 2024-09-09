namespace Shared.Models;

public abstract class BaseEncryptionKey
{
    public BaseEncryptionKey(byte[] key, byte[] iv)
    {
        Key = key;
        Iv = iv;
    }

    public byte[] Key { get; private set; }

    public byte[] Iv { get; private set; }
}
