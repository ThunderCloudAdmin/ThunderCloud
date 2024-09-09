using System.Security.Cryptography;

namespace Shared.Models;

public class RsaKeyPair
{
    public string PublicKey { get; set; }

    public string PrivateKey { get; set; }

    public RsaKeyPair(string publicKey, string privateKey)
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
}


