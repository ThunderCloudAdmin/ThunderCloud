namespace Shared.Models;

public class EncryptedBundle
{
    public EncryptedBundle(byte[] encryptedFile, byte[] encryptedTwoFishKey)
    {
        EncryptedFile = encryptedFile;
        EncryptedTwoFishKey = encryptedTwoFishKey;
    }

    public byte[] EncryptedFile { get; private set; }

    public byte[] EncryptedTwoFishKey { get; private set; }
}
