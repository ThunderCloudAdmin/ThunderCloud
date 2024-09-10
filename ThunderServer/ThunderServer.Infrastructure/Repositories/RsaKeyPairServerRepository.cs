using Microsoft.EntityFrameworkCore;
using ThunderServer.Infrastructure.Repositories.Interfaces;
using ThunderServer.Models;

namespace ThunderServer.Infrastructure.Repositories;

public class RsaKeyPairServerRepository : IRsaKeyPairServerRepository
{
    private readonly ThunderServerContext _context;

    public RsaKeyPairServerRepository(ThunderServerContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<RsaKeyPairServer> AddAsync(RsaKeyPairServer rsaKeyPairServer)
    {
        await this._context.AddAsync(rsaKeyPairServer);

        await this._context.SaveChangesAsync();

        return rsaKeyPairServer;
    }

    public async Task<RsaKeyPairServer> GetSingleAsync(Guid userGuid)
    {
        var encryptedPrivateKey = await this._context.RsaKeyPairServers.AsNoTracking().FirstOrDefaultAsync(x => x.User.Id == userGuid);

        return encryptedPrivateKey;
    }
}
