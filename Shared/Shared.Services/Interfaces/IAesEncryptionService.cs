using Shared.Models;

namespace Shared.Services.Interfaces;

public interface IAesEncryptionService : IEncryptionService<AesKey>
{
}
