namespace Shared.Models;

public class Tag
{
    public Guid TagId { get; set; } // Unique identifier for the tag
    public string TagName { get; set; } // The name of the tag
    public Guid UserId { get; set; } // Reference to the user who created the tag
    public Guid FileId { get; set; }
}
