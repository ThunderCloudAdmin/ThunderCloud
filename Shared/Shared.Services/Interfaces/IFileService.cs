using Shared.Models;

namespace Shared.Services.Interfaces;

public interface IFileService
{
    //Task<Guid> UploadFileAsync(IFormFile File, string FileName);
    //Task<DownloadFileResponse> GetFileAsync(Guid fileId);
    Task DeleteFileAsync(Guid fileId);
    Task<IEnumerable<ThunderFile>> GetUserFilesAsync(string userId);
    Task ShareFileAsync(Guid fileId, Guid recipientId, FilePermission permission);
    Task RevokeFileAccessAsync(Guid fileId, Guid recipientId);
    Task<IEnumerable<ThunderFile>> GetSharedFilesForUserAsync(Guid userId);
}