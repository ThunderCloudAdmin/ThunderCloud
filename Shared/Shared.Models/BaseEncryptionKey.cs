using System.Security.Cryptography;

namespace Shared.Models;

public abstract class BaseEncryptionKey
{
    public byte[] Key { get; private set; }

    public byte[] Iv { get; private set; }

    public BaseEncryptionKey(byte[] key, byte[] iv)
    {
        Key = key;
        Iv = iv;
    }

    /// <summary>
    /// Can be of sizes 16, 24 or 32
    /// </summary>
    /// <param name="size"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    protected static byte[] GenerateRandomKeyBytes(int size)
    {
        if (size != 16 && size != 24 && size != 32)
        {
            throw new ArgumentException("Invalid key size. Use 16, 24, or 32 bytes.");
        }

        byte[] randomBytes = new byte[size];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomBytes);
        }
        return randomBytes;
    }

    /// <summary>
    ///  Can be of size 16
    /// </summary>
    /// <param name="size"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    protected static byte[] GenerateRandomIvBytes(int size)
    {
        if (size != 16)
        {
            throw new ArgumentException("Invalid iv size. Use 16 bytes.");
        }

        byte[] randomBytes = new byte[size];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomBytes);
        }
        return randomBytes;
    }
}
