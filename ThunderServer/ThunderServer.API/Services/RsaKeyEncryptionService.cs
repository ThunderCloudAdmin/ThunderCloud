using System.Security.Cryptography;
using System.Text;
using ThunderServer.API.Services.Interfaces;
using ThunderServer.Infrastructure.Repositories.Interfaces;
using ThunderServer.Models;

namespace ThunderServer.API.Services;

public class RsaKeyEncryptionService : IRsaKeyEncryptionService
{
    private readonly IRsaKeyPairServerRepository rsaKeyPairServerRepository;

    public RsaKeyEncryptionService(IRsaKeyPairServerRepository rsaKeyPairServerRepository)
    {
        this.rsaKeyPairServerRepository = rsaKeyPairServerRepository ?? throw new ArgumentNullException(nameof(rsaKeyPairServerRepository));
    }

    public async Task EncryptPrivateKeyWithArgon2Key(ThunderUser user, Argon2Key argon2Key)
    {
        var rsaKeyPair = RsaKeyPairServer.GenerateRsaKeyPair();

        var privateKey = Encoding.UTF8.GetBytes(rsaKeyPair.PrivateKey);

        byte[] encryptedPrivateKey;

        using (var aes = Aes.Create())
        {
            aes.Key = argon2Key.DerivedKey;
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

                encryptedPrivateKey = ms.ToArray(); // Return the encrypted private key (with IV prepended)
            }
        }

        rsaKeyPair.EncryptedPrivateRsaKey = encryptedPrivateKey;
        rsaKeyPair.Argon2Key = argon2Key;
        rsaKeyPair.User = user;

        await this.rsaKeyPairServerRepository.AddAsync(rsaKeyPair);
    }

    public async Task<byte[]> DecryptPrivateKeyWithArgon2Key(Guid userGuid)
    {
        var encryptedPrivateKey = await this.rsaKeyPairServerRepository.GetSingleAsync(userGuid);

        using (var aes = Aes.Create())
        {
            aes.Key = encryptedPrivateKey.Argon2Key.Salt;

            // Extract the IV from the encrypted data (first 16 bytes)
            byte[] iv = new byte[aes.BlockSize / 8];
            Array.Copy(encryptedPrivateKey.EncryptedPrivateRsaKey, 0, iv, 0, iv.Length);
            aes.IV = iv;

            using (var decryptor = aes.CreateDecryptor())
            using (var ms = new MemoryStream(encryptedPrivateKey.EncryptedPrivateRsaKey, iv.Length, encryptedPrivateKey.EncryptedPrivateRsaKey.Length - iv.Length))
            using (var cryptoStream = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
            using (var resultStream = new MemoryStream())
            {
                cryptoStream.CopyTo(resultStream);
                return resultStream.ToArray();  // Return the decrypted private key
            }
        }
    }
}
