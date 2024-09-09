public class LocalStorageProvider : IStorageProvider
{
    private readonly string _baseDirectory;

    public LocalStorageProvider(string baseDirectory)
    {
        _baseDirectory = baseDirectory;
        Directory.CreateDirectory(_baseDirectory);
    }

    public async Task SaveFileAsync(string filePath, byte[] content)
    {
        var fullPath = Path.Combine(_baseDirectory, filePath);
        await File.WriteAllBytesAsync(fullPath, content);
    }

    public async Task<byte[]> ReadFileAsync(string filePath)
    {
        var fullPath = Path.Combine(_baseDirectory, filePath);
        return await File.ReadAllBytesAsync(fullPath);
    }

    public async Task DeleteFileAsync(string filePath)
    {
        var fullPath = Path.Combine(_baseDirectory, filePath);
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }
    }
}
