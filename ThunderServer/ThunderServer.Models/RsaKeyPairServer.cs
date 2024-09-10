using Shared.Models;
using System.Security.Cryptography;

namespace ThunderServer.Models;

public class RsaKeyPairServer : RsaKeyPair
{
    protected RsaKeyPairServer() : base() { }

    private RsaKeyPairServer(string publicKey, string privateKey)
    {
        PublicKey = publicKey;
        PrivateKey = privateKey;
    }

    public byte[] EncryptedPrivateRsaKey { get; set; }

    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    //Navigations
    public ThunderUser User { get; set; }

    public Argon2Key Argon2Key { get; set; }

    public static RsaKeyPairServer GenerateRsaKeyPair(int keySize = 4096)
    {
        string privateKey;
        // Export the public key to PEM format
        string publicKey;

        using (RSA rsa = RSA.Create(keySize))
        {
            // Export the private key to PEM format
            privateKey = ExportPrivateKey(rsa);
            // Export the public key to PEM format
            publicKey = ExportPublicKey(rsa);

            return new RsaKeyPairServer(publicKey, privateKey);

            // Save the keys to files
            File.WriteAllText("privateKey.pem", privateKey);
            File.WriteAllText("publicKey.pem", publicKey);

            Console.WriteLine("RSA key pair generated and saved.");
        }

        return new RsaKeyPairServer(publicKey, privateKey);
    }
}