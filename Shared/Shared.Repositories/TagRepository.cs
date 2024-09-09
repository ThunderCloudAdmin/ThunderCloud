using Microsoft.EntityFrameworkCore;
using Shared.Models;
using Shared.Repositories;
using Shared.Repositories.Interfaces;

public class TagRepository
{
    private readonly CommonDbContext _context;

    public TagRepository(CommonDbContext context)
    {
        _context = context;
    }

    //public async Task AddAsync(Tag tag)
    //{
    //    _context.Tags.Add(tag);
    //    await _context.SaveChangesAsync();
    //}

    //public async Task<Tag> GetByFileIdAndTagNameAsync(Guid fileId, string tagName)
    //{
    //    return await _context.Tags
    //        .FirstOrDefaultAsync(t => t.FileId == fileId && t.TagName == tagName);
    //}

    //public async Task DeleteAsync(Tag tag)
    //{
    //    _context.Tags.Remove(tag);
    //    await _context.SaveChangesAsync();
    //}

    //public async Task<IEnumerable<string>> GetTagsByFileIdAsync(Guid fileId)
    //{
    //    return await _context.Tags
    //        .Where(t => t.FileId == fileId)
    //        .Select(t => t.TagName)
    //        .ToListAsync();
    //}
}
