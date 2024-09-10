using ThunderServer.Models;

namespace ThunderServer.Infrastructure.Repositories.Interfaces;

public interface IRsaKeyPairServerRepository
{
    Task<RsaKeyPairServer> AddAsync(RsaKeyPairServer rsaKeyPairServer);

    Task<RsaKeyPairServer> GetSingleAsync(Guid userGuid);
}
