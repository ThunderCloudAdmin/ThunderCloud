namespace Shared.Models;

public class AesKey : BaseEncryptionKey
{
    private AesKey(byte[] key, byte[] iv) : base(key, iv) { }

    public static AesKey GenerateEncryptionKey(int keySize = 16, int ivSize = 16)
    {
        if (keySize != 16 && keySize != 32)
        {
            throw new ArgumentException("Key Size used is not supported, use only 16 or 32");
        }

        if (ivSize != 16)
        {
            throw new ArgumentException("Iv Size used is not supported, use only 32 or 64");
        }

        byte[] key = GenerateRandomKeyBytes(keySize);
        byte[] iv = GenerateRandomIvBytes(ivSize);

        return new AesKey(key, iv);
    }
}
