using Shared.Models;

namespace Shared.Services.Publishers.Interfaces;

public interface IFileUploadService
{
    Task UploadFileAsync(EncryptedBundle encryptedBundle);
}