using System.Security.Cryptography;

namespace Shared.Models;

public class RsaKeyPair
{
    protected RsaKeyPair() { }

    public string PublicKey { get; set; }

    public string PrivateKey { get; set; }

    private RsaKeyPair(string publicKey, string privateKey)
    {
        PublicKey = publicKey;
        PrivateKey = privateKey;
    }

    public RSA LoadPublicKeyAsRSA()
    {
        var publicKeyDer = Convert.FromBase64String(PublicKey);

        //encrypt 2fish key with public RSA Key
        var rsa = RSA.Create();

        rsa.ImportSubjectPublicKeyInfo(publicKeyDer, out _);
        rsa.ExportParameters(false); // false for public key only

        return rsa;
    }
    
    public RSA LoadPrivateKeyAsRSA()
    {
        var privateKeyDer = Convert.FromBase64String(PrivateKey);

        //encrypt 2fish key with public RSA Key
        var rsa = RSA.Create();

        rsa.ImportRSAPrivateKey(privateKeyDer, out _);
        rsa.ExportParameters(true); // false for public key only

        return rsa;
    }

    public static RsaKeyPair GenerateRsaKeyPair(int keySize = 4096)
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


            return new RsaKeyPair(publicKey, privateKey);

            // Save the keys to files
            File.WriteAllText("privateKey.pem", privateKey);
            File.WriteAllText("publicKey.pem", publicKey);

            Console.WriteLine("RSA key pair generated and saved.");
        }

        return new RsaKeyPair(publicKey, privateKey);
    }

    public static string ExportPrivateKey(RSA rsa)
    {
        var privateKeyBytes = rsa.ExportRSAPrivateKey();
        return Convert.ToBase64String(privateKeyBytes);
    }

    // Export public key in PEM format
    public static string ExportPublicKey(RSA rsa)
    {
        var publicKeyBytes = rsa.ExportSubjectPublicKeyInfo();
        return Convert.ToBase64String(publicKeyBytes);
    }
}
