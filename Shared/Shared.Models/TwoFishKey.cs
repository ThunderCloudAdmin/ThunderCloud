namespace Shared.Models;

public class TwoFishKey : BaseEncryptionKey
{
    private TwoFishKey(byte[] key, byte[] iv) : base(key, iv) { }

    public static TwoFishKey GenerateEncryptionKey(int keySize = 32, int ivSize = 16)
    {
        if (keySize != 32 && keySize != 64)
        {
            throw new ArgumentException("Key Size used is not supported, use only 32 or 64");
        }
        if (ivSize != 16)
        {
            throw new ArgumentException("Iv Size used is not supported, use only 32 or 64");
        }

        byte[] key = GenerateRandomKeyBytes(keySize);
        byte[] iv = GenerateRandomIvBytes(ivSize);

        return new TwoFishKey(key, iv);
    }
}
