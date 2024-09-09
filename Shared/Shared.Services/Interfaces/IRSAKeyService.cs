using Shared.Models;
using System.Security.Cryptography;

namespace Shared.Services.Interfaces;

public interface IRSAKeyService
{
    byte[] EncryptAESKeyWithRSA(byte[] aesKey, RSA recipientRsaPublicKey);

    byte[] DecryptAESKeyWithRSA(byte[] encryptedAesKey, RSA recipientRsaPrivateKey);

    RsaKeyPair RetrievePublicKey(int keySize = 4096);
}
