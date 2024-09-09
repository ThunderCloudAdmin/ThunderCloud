namespace Shared.Repositories;

public class DirectoryRepository
{
    private readonly CommonDbContext _context;

    public DirectoryRepository(CommonDbContext context)
    {
        _context = context;
    }

    //public async Task AddAsync(ThunderFolder directory)
    //{
    //    _context.Directories.Add(directory);
    //    await _context.SaveChangesAsync();
    //}

    //public async Task<ThunderFolder> GetByIdAsync(Guid directoryId)
    //{
    //    return await _context.Directories.FindAsync(directoryId);
    //}

    //public async Task<IEnumerable<ThunderFolder>> GetDirectoriesByUserAsync(Guid userId)
    //{
    //    return await _context.Directories
    //        .Where(d => d.UserId == userId)
    //        .Select(d => new DirectoryDto
    //        {
    //            DirectoryId = d.DirectoryId,
    //            DirectoryName = d.DirectoryName,
    //            ParentDirectoryId = d.ParentDirectoryId
    //        })
    //        .ToListAsync();
    //}
}