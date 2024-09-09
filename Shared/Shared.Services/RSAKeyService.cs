using Shared.Models;
using Shared.Services.Interfaces;
using System.Security.Cryptography;

namespace Shared.Services;

public class RSAKeyService : IRSAKeyService
{
    public byte[] EncryptAESKeyWithRSA(byte[] aesKey, RSA recipientRsaPublicKey)
    {
        return recipientRsaPublicKey.Encrypt(aesKey, RSAEncryptionPadding.OaepSHA256);
    }

    public byte[] DecryptAESKeyWithRSA(byte[] encryptedAesKey, RSA recipientRsaPrivateKey)
    {
        return recipientRsaPrivateKey.Decrypt(encryptedAesKey, RSAEncryptionPadding.OaepSHA256);
    }

    public RsaKeyPair RetrievePublicKey(int keySize = 4096)
    {
        using (RSA rsa = RSA.Create(keySize))
        {
            // Export the private key to PEM format
            string privateKey = ExportPrivateKey(rsa);
            // Export the public key to PEM format
            string publicKey = ExportPublicKey(rsa);


            return new RsaKeyPair(publicKey, privateKey);

            // Save the keys to files
            File.WriteAllText("privateKey.pem", privateKey);
            File.WriteAllText("publicKey.pem", publicKey);

            Console.WriteLine("RSA key pair generated and saved.");
        }
    }

    private static string ExportPrivateKey(RSA rsa)
    {
        var privateKeyBytes = rsa.ExportRSAPrivateKey();
        return ConvertToString(privateKeyBytes);
    }

    // Export public key in PEM format
    private static string ExportPublicKey(RSA rsa)
    {
        var publicKeyBytes = rsa.ExportSubjectPublicKeyInfo();
        return ConvertToString(publicKeyBytes);
    }

    // Helper function to convert byte array to PEM format
    private static string ConvertToPem(byte[] keyBytes, string keyType)
    {
        var base64Key = Convert.ToBase64String(keyBytes);
        var pemKey = $"-----BEGIN {keyType}-----\n{base64Key}\n-----END {keyType}-----";
        return pemKey;
    }

    private static string ConvertToString(byte[] keyBytes)
    {
        return Convert.ToBase64String(keyBytes);
    }
}
