namespace ThunderServer.API.Services.Interfaces;

public interface IRsaKeyEncryptionService
{
    byte[] EncryptPrivateKeyWithArgon2Key(byte[] argon2Key);

    byte[] DecryptPrivateKeyWithArgon2Key(byte[] encryptedPrivateKey, byte[] argon2Key);
}
