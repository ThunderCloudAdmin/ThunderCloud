using Shared.Models;

namespace Shared.Repositories.Interfaces;

public interface IDirectoryRepository
{
    Task AddAsync(ThunderFolder directory);
    Task<ThunderFolder> GetByIdAsync(Guid directoryId);
    Task<IEnumerable<ThunderFolder>> GetDirectoriesByUserAsync(Guid userId);
}
