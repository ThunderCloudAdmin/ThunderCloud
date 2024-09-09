using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Shared.Models;
using Shared.Services.Interfaces;

namespace Shared.Services;

public class AesEncryptionService : IAesEncryptionService
{
    public byte[] Decrypt(byte[] data, AesKey key)
    {
        var cipher = CipherUtilities.GetCipher("AES/CBC/PKCS7Padding");

        cipher.Init(false, new ParametersWithIV(new KeyParameter(key.Key), key.Iv));

        return cipher.DoFinal(data);
    }

    public byte[] Encrypt(byte[] data, AesKey key)
    {
        var cipher = CipherUtilities.GetCipher("AES/CBC/PKCS7Padding");

        cipher.Init(true, new ParametersWithIV(new KeyParameter(key.Key), key.Iv));

        return cipher.DoFinal(data);
    }
}
