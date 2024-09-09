using Shared.Models;

namespace Shared.Repositories.Interfaces;

public interface IEncryptionKeyRepository
{
    Task AddAsync(EncryptionKey encryptionKey);
    Task<EncryptionKey> GetByFileIdAsync(Guid fileId);
}