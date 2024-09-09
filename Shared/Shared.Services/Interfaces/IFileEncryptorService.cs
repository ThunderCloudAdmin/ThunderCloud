using Shared.Models;

namespace Shared.Services.Interfaces;

public interface IFileEncryptorService
{
    EncryptedBundle GenerateEncryptedFile(byte[] dataToEncrypt);
}
