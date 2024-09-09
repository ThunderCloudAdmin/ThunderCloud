using Microsoft.EntityFrameworkCore;
using Shared.Repositories;
using Shared.Repositories.Interfaces;

public class FileRepository 
{
    private readonly CommonDbContext _context;

    public FileRepository(CommonDbContext context)
    {
        _context = context;
    }

    //public async Task AddAsync(File file)
    //{
    //    _context.Files.Add(file);
    //    await _context.SaveChangesAsync();
    //}

    //public async Task<File> GetByIdAsync(Guid fileId)
    //{
    //    return await _context.Files.FindAsync(fileId);
    //}

    //public async Task DeleteAsync(File file)
    //{
    //    _context.Files.Remove(file);
    //    await _context.SaveChangesAsync();
    //}

    //public async Task UpdateAsync(File file)
    //{
    //    _context.Files.Update(file);
    //    await _context.SaveChangesAsync();
    //}

    //public async Task<IEnumerable<File>> GetFilesByUserAsync(Guid userId)
    //{
    //    return await _context.Files
    //        .Where(f => f.UserId == userId)
    //        .Select(f => new FileDto
    //        {
    //            FileId = f.FileId,
    //            FileName = f.FileName,
    //            FileSize = f.FileSize,
    //            UploadDate = f.UploadDate
    //        })
    //        .ToListAsync();
    //}

    //public async Task AddFileShareAsync(ThunderFileShare fileShare)
    //{
    //    _context.FileShares.Add(fileShare);
    //    await _context.SaveChangesAsync();
    //}

    //public async Task<ThunderFileShare> GetFileShareAsync(Guid fileId, Guid userId)
    //{
    //    return await _context.FileShares
    //        .FirstOrDefaultAsync(fs => fs.FileId == fileId && fs.UserId == userId);
    //}

    //public async Task DeleteFileShareAsync(ThunderFileShare fileShare)
    //{
    //    _context.FileShares.Remove(fileShare);
    //    await _context.SaveChangesAsync();
    //}

    //public async Task<IEnumerable<File>> GetSharedFilesByUserAsync(Guid userId)
    //{
    //    return await _context.FileShares
    //        .Where(fs => fs.UserId == userId)
    //        .Select(fs => new FileDto
    //        {
    //            FileId = fs.File.FileId,
    //            FileName = fs.File.FileName,
    //            FileSize = fs.File.FileSize,
    //            UploadDate = fs.File.UploadDate
    //        })
    //        .ToListAsync();
    //}
}
