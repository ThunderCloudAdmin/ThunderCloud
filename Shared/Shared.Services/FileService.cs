using Shared.Models;
using Shared.Repositories.Interfaces;
using Shared.Services.Interfaces;

namespace Shared.Services;

public class FileService
{
    private readonly IThunderFileRepository _fileRepository;
    //private readonly IEncryptionService _encryptionService;
    private readonly IStorageProvider _storageProvider;
    private readonly IEncryptionKeyRepository _encryptionKeyRepository;

    public FileService(
        IThunderFileRepository fileRepository,
        IEncryptionKeyRepository encryptionKeyRepository,
        //IEncryptionService encryptionService,
        IStorageProvider storageProvider)
    {
        _fileRepository = fileRepository;
        _encryptionKeyRepository = encryptionKeyRepository;
        //_encryptionService = encryptionService;
        _storageProvider = storageProvider;
    }

    //public async Task UploadFileAsync(Guid fileId, byte[] fileData)
    //{
    //    // Generate a new encryption key
    //    var key = Convert.ToBase64String(Guid.NewGuid().ToByteArray()); // For demonstration; use a secure key generation method
    //    var encryptionKey = new EncryptionKey
    //    {
    //        Id = Guid.NewGuid(),
    //        FileId = fileId,
    //        Key = key,
    //        CreatedDate = DateTime.UtcNow
    //    };

    //    // Encrypt file data
    //    var encryptedData = await _encryptionService.EncryptAsync(fileData, key);

    //    // Save encrypted data to storage
    //    await _storageProvider.SaveFileAsync(fileId.ToString(), encryptedData);

    //    // Store encryption key in the database
    //    await _encryptionKeyRepository.AddAsync(encryptionKey);

    //    // Optionally, save file metadata in the database
    //    var file = new ThunderFile { Id = fileId, FileName = "example.txt" };
    //    await _fileRepository.AddAsync(file);
    //}

    //public async Task<byte[]> DownloadFileAsync(Guid fileId)
    //{
    //    // Retrieve encryption key from the database
    //    var encryptionKey = await _encryptionKeyRepository.GetByFileIdAsync(fileId);
    //    if (encryptionKey == null)
    //    {
    //        throw new InvalidOperationException("Encryption key not found.");
    //    }

    //    // Read encrypted data from storage
    //    var encryptedData = await _storageProvider.ReadFileAsync(fileId.ToString());

    //    // Decrypt data
    //    return await _encryptionService.DecryptAsync(encryptedData, encryptionKey.Key);
    //}
}