using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Shared.Models;
using Shared.Services.Interfaces;

namespace Shared.Services;

public class TwoFishEncryptionService : ITwoFishEncryptionService
{
    //For more safety implement the GCM Cipher instead of CBC
    //https://www.scottbrady91.com/c-sharp/aes-gcm-dotnet

    public byte[] Decrypt(byte[] data, TwoFishKey key)
    {
        var cipher = CipherUtilities.GetCipher("TwoFish/CBC/PKCS7Padding");

        cipher.Init(false, new ParametersWithIV(new KeyParameter(key.Key), key.Iv));

        return cipher.DoFinal(data);
    }

    public byte[] Encrypt(byte[] data, TwoFishKey key)
    {
        var cipher = CipherUtilities.GetCipher("TwoFish/CBC/PKCS7Padding");

        cipher.Init(true, new ParametersWithIV(new KeyParameter(key.Key), key.Iv));

        return cipher.DoFinal(data);
    }
}
