using Shared.Repositories.Interfaces;
using Shared.Services.Interfaces;

namespace Shared.Services;

public class DirectoryService 
{
    private readonly IDirectoryRepository _directoryRepository;
    private readonly IThunderFileRepository _fileRepository;

    public DirectoryService(IDirectoryRepository directoryRepository, IThunderFileRepository fileRepository)
    {
        _directoryRepository = directoryRepository;
        _fileRepository = fileRepository;
    }

    //public async Task<Guid> CreateDirectoryAsync(string directoryName, Guid? parentDirectoryId)
    //{
    //    if (string.IsNullOrWhiteSpace(directoryName))
    //        throw new ArgumentException("Directory name cannot be empty.");

    //    var directoryId = Guid.NewGuid();
    //    var directory = new Directory
    //    {
    //        DirectoryId = directoryId,
    //        DirectoryName = directoryName,
    //        ParentDirectoryId = parentDirectoryId,
    //        UserId = /* Your logic to get the current user ID */
    //        //CreatedDate = DateTime.UtcNow
    //    };

    //    await _directoryRepository.AddAsync(directory);
    //    return directoryId;
    //}

    //public async Task<IEnumerable<Directory>> GetUserDirectoriesAsync(Guid userId)
    //{
    //    return await _directoryRepository.GetDirectoriesByUserAsync(userId);
    //}

    //public async Task MoveFileToDirectoryAsync(Guid fileId, Guid directoryId)
    //{
    //    var file = await _fileRepository.GetByIdAsync(fileId);
    //    if (file == null)
    //        throw new FileNotFoundException("File not found.");

    //    var directory = await _directoryRepository.GetByIdAsync(directoryId);
    //    if (directory == null)
    //        throw new DirectoryNotFoundException("Directory not found.");

    //    file.DirectoryId = directoryId;
    //    await _fileRepository.UpdateAsync(file);
    //}
}
