using Shared.Models;

namespace Shared.Repositories.Interfaces;

public interface ITagRepository
{
    Task AddAsync(Tag tag);
    Task<Tag> GetByFileIdAndTagNameAsync(Guid fileId, string tagName);
    Task DeleteAsync(Tag tag);
    Task<IEnumerable<string>> GetTagsByFileIdAsync(Guid fileId);
}