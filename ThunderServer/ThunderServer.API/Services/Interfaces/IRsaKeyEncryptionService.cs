namespace ThunderServer.API.Services.Interfaces;

public interface IRsaKeyEncryptionService
{
    Task EncryptPrivateKeyWithArgon2Key(byte[] argon2Key);

    Task<byte[]> DecryptPrivateKeyWithArgon2Key(Guid userGuid);
}
