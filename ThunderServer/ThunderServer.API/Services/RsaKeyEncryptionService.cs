using Shared.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;
using ThunderServer.API.Services.Interfaces;
using ThunderServer.Infrastructure;
using ThunderServer.Models;

namespace ThunderServer.API.Services;

public class RsaKeyEncryptionService : IRsaKeyEncryptionService
{
    private readonly IRSAKeyService rSAKeyService;
    private readonly ThunderServerContext serverContext;

    public RsaKeyEncryptionService(IRSAKeyService rSAKeyService, ThunderServerContext serverContext)
    {
        this.serverContext = serverContext ?? throw new ArgumentNullException(nameof(serverContext));
    }    

    public byte[] EncryptPrivateKeyWithArgon2Key(byte[] argon2Key)
    {        
        var rsaKeyPair = (RsaKeyPairServer)this.rSAKeyService.GenerateRsaKeyPair();

        var privateKey = Encoding.UTF8.GetBytes(rsaKeyPair.PrivateKey);

        using (var aes = Aes.Create())
        {
            aes.Key = argon2Key;
            aes.GenerateIV();  // Generate a new IV (Initialization Vector)

            using (var encryptor = aes.CreateEncryptor())
            using (var ms = new MemoryStream())
            {
                // Write the IV to the beginning of the encrypted output
                ms.Write(aes.IV, 0, aes.IV.Length);

                using (var cryptoStream = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(privateKey, 0, privateKey.Length);
                }

                return ms.ToArray(); // Return the encrypted private key (with IV prepended)
            }
        }
    }

    public byte[] DecryptPrivateKeyWithArgon2Key(byte[] encryptedPrivateKey, byte[] argon2Key)
    {
        using (var aes = Aes.Create())
        {
            aes.Key = argon2Key;

            // Extract the IV from the encrypted data (first 16 bytes)
            byte[] iv = new byte[aes.BlockSize / 8];
            Array.Copy(encryptedPrivateKey, 0, iv, 0, iv.Length);
            aes.IV = iv;

            using (var decryptor = aes.CreateDecryptor())
            using (var ms = new MemoryStream(encryptedPrivateKey, iv.Length, encryptedPrivateKey.Length - iv.Length))
            using (var cryptoStream = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
            using (var resultStream = new MemoryStream())
            {
                cryptoStream.CopyTo(resultStream);
                return resultStream.ToArray();  // Return the decrypted private key
            }
        }
    }
}
