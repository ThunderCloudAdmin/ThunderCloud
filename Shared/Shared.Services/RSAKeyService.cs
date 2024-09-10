using Shared.Services.Interfaces;
using System.Security.Cryptography;

namespace Shared.Services;

public class RSAKeyService : IRSAKeyService
{
    public byte[] EncryptSymmetricKeyWithRSA(byte[] aesKey, RSA recipientRsaPublicKey)
    {
        return recipientRsaPublicKey.Encrypt(aesKey, RSAEncryptionPadding.OaepSHA256);
    }

    public byte[] DecryptSymmetricKeyWithRSA(byte[] encryptedAesKey, RSA recipientRsaPrivateKey)
    {
        return recipientRsaPrivateKey.Decrypt(encryptedAesKey, RSAEncryptionPadding.OaepSHA256);
    }

    // Helper function to convert byte array to PEM format
    private static string ConvertToPem(byte[] keyBytes, string keyType)
    {
        var base64Key = Convert.ToBase64String(keyBytes);
        var pemKey = $"-----BEGIN {keyType}-----\n{base64Key}\n-----END {keyType}-----";
        return pemKey;
    }
}
