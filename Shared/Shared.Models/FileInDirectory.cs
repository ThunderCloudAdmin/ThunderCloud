namespace Shared.Models;

public class FileInDirectory
{
    public Guid FileInDirectoryId { get; set; } // Unique identifier for the relationship
    public Guid FileId { get; set; } // Reference to the file
    public Guid DirectoryId { get; set; } // Reference to the directory where the file is located
}