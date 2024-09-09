using Shared.Models;
using System.Security.Cryptography;

namespace Shared.Services.Interfaces;

public interface IRSAKeyService
{
    byte[] EncryptSymmetricKeyWithRSA(byte[] aesKey, RSA recipientRsaPublicKey);

    byte[] DecryptSymmetricKeyWithRSA(byte[] encryptedAesKey, RSA recipientRsaPrivateKey);

    RsaKeyPair GenerateRsaKeyPair(int keySize = 4096);
}
