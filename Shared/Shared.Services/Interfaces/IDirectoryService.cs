using Shared.Models;

namespace Shared.Services.Interfaces;

public interface IDirectoryService
{
    Task<Guid> CreateDirectoryAsync(string directoryName, Guid? parentDirectoryId);
    Task<IEnumerable<ThunderFolder>> GetUserDirectoriesAsync(Guid userId);
    Task MoveFileToDirectoryAsync(Guid fileId, Guid directoryId);
}