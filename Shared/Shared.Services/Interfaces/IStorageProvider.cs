public interface IStorageProvider
{
    Task SaveFileAsync(string filePath, byte[] content);
    Task<byte[]> ReadFileAsync(string filePath);
    Task DeleteFileAsync(string filePath);
}
