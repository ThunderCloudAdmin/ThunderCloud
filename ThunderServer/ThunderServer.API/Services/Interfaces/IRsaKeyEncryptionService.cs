using ThunderServer.Models;

namespace ThunderServer.API.Services.Interfaces;

public interface IRsaKeyEncryptionService
{
    Task EncryptPrivateKeyWithArgon2Key(ThunderUser user, Argon2Key argon2Key);

    Task<byte[]> DecryptPrivateKeyWithArgon2Key(Guid userGuid);
}
