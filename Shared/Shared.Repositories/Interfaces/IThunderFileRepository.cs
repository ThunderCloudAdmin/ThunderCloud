using Shared.Models;

namespace Shared.Repositories.Interfaces;

public interface IThunderFileRepository
{
    Task AddAsync(ThunderFile file);
    Task<ThunderFile> GetByIdAsync(Guid fileId);
    Task DeleteAsync(ThunderFile file);
    Task UpdateAsync(ThunderFile file);
    Task<IEnumerable<ThunderFile>> GetFilesByUserAsync(Guid userId);
    Task AddFileShareAsync(ThunderFileShare fileShare);
    Task<ThunderFileShare> GetFileShareAsync(Guid fileId, Guid userId);
    Task DeleteFileShareAsync(ThunderFileShare fileShare);
    Task<IEnumerable<ThunderFile>> GetSharedFilesByUserAsync(Guid userId);
}