namespace Shared.Models;

public class ThunderFolder
{
    public ThunderFolder() { }


	private ThunderFolder(string name, Guid? parentFolderId, DateTime utcNow)
    {
        Name = name;
        ParentFolderId = parentFolderId;
        CreatedAt = utcNow;
        IsRootFolder = parentFolderId.HasValue;
    }

	public ThunderFolder(Guid id, string name, Guid? parentFolderId, DateTime createdAt, bool isRootFolder, ICollection<ThunderFile> files)
	{
		Id = id;
		Name = name ?? throw new ArgumentNullException(nameof(name));
		ParentFolderId = parentFolderId;
		CreatedAt = createdAt;
		IsRootFolder = isRootFolder;
		Files = files ?? throw new ArgumentNullException(nameof(files));
	}

	public Guid Id { get; init; }
    public string Name { get; init; }
    public Guid? ParentFolderId { get; init; } // Nullable for root folder
    public DateTime CreatedAt { get; init; }
    public bool IsRootFolder { get; init; }

    public static ThunderFolder Create(string name, Guid? parentFolderId)
    {
        return new ThunderFolder(name, parentFolderId, DateTime.UtcNow);
    }

    // Navigation properties
    public virtual ICollection<ThunderFile> Files { get; init; }
   
}
