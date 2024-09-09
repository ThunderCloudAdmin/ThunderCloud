using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities.Encoders;
using Shared.Models;
using Shared.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace Shared.Services;

public class TwoFishEncryptionService : ITwoFishEncryptionService
{
    //For more safety implement the GCM Cipher instead of CBC
    //https://www.scottbrady91.com/c-sharp/aes-gcm-dotnet

    public byte[] Decrypt(byte[] data, TwoFishKey key)
    {
        var cipher = CipherUtilities.GetCipher("TwoFish/CBC/PKCS7Padding");

        cipher.Init(true, new ParametersWithIV(new KeyParameter(key.Key), key.Iv));

        return cipher.DoFinal(data);
    }

    public byte[] Encrypt(byte[] data, TwoFishKey key)
    {
        var cipher = CipherUtilities.GetCipher("TwoFish/CBC/PKCS7Padding");

        cipher.Init(true, new ParametersWithIV(new KeyParameter(key.Key), key.Iv));

        return cipher.DoFinal(data);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="keySize">Corresponds to 256 bits key</param>
    /// <param name="ivSize">Corresponds to 128 bits key</param>
    /// <returns></returns>
    public TwoFishKey GenerateEncryptionKey(int keySize = 32, int ivSize = 16)
    {
        byte[] key = GenerateRandomKeyBytes(keySize);
        byte[] iv = GenerateRandomIvBytes(ivSize);

        return new TwoFishKey(key, iv);
    }

    private static PaddedBufferedBlockCipher GenerateEncryptCipher(TwoFishKey twoFishKey)
    {
        string key1 = "000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f";
        string key2 = "000102030405060708090a0b0c0d0e0f1011121314151617";

        Encoding.UTF8.GetBytes(key1);

        var engine = new TwofishEngine();
        var cipher = new PaddedBufferedBlockCipher(engine, new Pkcs7Padding());

        cipher.Init(false, new ParametersWithIV(new KeyParameter(Hex.Decode(key1)), Encoding.UTF8.GetBytes("8ef0272c42db838bcf7b07af0ec30f38")));

        return cipher;
    }

    private static PaddedBufferedBlockCipher GenerateCipher(TwoFishKey twoFishKey)
    {
        // Initialize Twofish cipher
        PaddedBufferedBlockCipher cipher = new PaddedBufferedBlockCipher(new CbcBlockCipher(new TwofishEngine()));
        cipher.Init(false, new ParametersWithIV(new KeyParameter(twoFishKey.Key), twoFishKey.Iv));

        return cipher;
    }

    private static byte[] PerformCryptography(byte[] input, PaddedBufferedBlockCipher cipher)
    {
        //byte[] output = new byte[cipher.GetOutputSize(input.Length)];
        //int length = cipher.ProcessBytes(input, 0, input.Length, output, 0);
        //length += cipher.DoFinal(output, length);

        byte[] output = new byte[cipher.GetOutputSize(input.Length)];
        int length = cipher.ProcessBytes(input, 0, input.Length, output, 0);
        length += cipher.DoFinal(output, length);

        byte[] result = new byte[length];
        Array.Resize(ref output, length);

        return result;
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
