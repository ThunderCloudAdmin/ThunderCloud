using ThunderServer.API.Services.Interfaces;
using ThunderServer.Infrastructure;
using ThunderServer.Models;

namespace ThunderServer.API.Services;

public class Argon2KeyRepository : IArgon2KeyRepository
{
    private readonly ThunderServerContext _context;

    public Argon2KeyRepository(ThunderServerContext context)
    {
        this._context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<Argon2Key> AddAsync(Argon2Key argonKey)
    {
        await this._context.Argon2Keys.AddAsync(argonKey);

        await this._context.SaveChangesAsync();

        return argonKey;
    }
}
