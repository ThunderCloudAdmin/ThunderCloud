using Microsoft.EntityFrameworkCore;
using Shared.Models;
using Shared.Repositories.Interfaces;

namespace Shared.Repositories;

public class EncryptionKeyRepository : IEncryptionKeyRepository
{
    private readonly CommonDbContext _context;

    public EncryptionKeyRepository(CommonDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(EncryptionKey encryptionKey)
    {
        await _context.EncryptionKeys.AddAsync(encryptionKey);
        await _context.SaveChangesAsync();
    }

    public async Task<EncryptionKey> GetByFileIdAsync(Guid fileId)
    {
        return await _context.EncryptionKeys
            .FirstOrDefaultAsync(e => e.FileId == fileId);
    }
}