using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Shared.Models;
using Shared.Services.Interfaces;
using System.Security.Cryptography;

namespace Shared.Services;

public class AesEncryptionService : IAesEncryptionService
{
    public byte[] Decrypt(byte[] data, AesKey key)
    {
        var cipher = CipherUtilities.GetCipher("AES/CBC/PKCS7Padding");

        cipher.Init(true, new ParametersWithIV(new KeyParameter(key.Key), key.Iv));

        return cipher.DoFinal(data);
    }

    public byte[] Encrypt(byte[] data, AesKey key)
    {
        var cipher = CipherUtilities.GetCipher("AES/CBC/PKCS7Padding");

        cipher.Init(true, new ParametersWithIV(new KeyParameter(key.Key), key.Iv));

        return cipher.DoFinal(data);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="keySize">Corresponds to 256 bits key</param>
    /// <param name="ivSize">Corresponds to 128 bits key</param>
    /// <returns></returns>
    public AesKey GenerateEncryptionKey(int keySize = 16, int ivSize = 16)
    {
        byte[] key = GenerateRandomKeyBytes(keySize);
        byte[] iv = GenerateRandomIvBytes(ivSize);

        return new AesKey(key, iv);
    }

    private static byte[] GenerateRandomKeyBytes(int size)
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

    private static byte[] GenerateRandomIvBytes(int size)
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
