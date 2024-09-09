namespace Shared.Models;
public class ThunderFileTag
{
    public Guid FileTagId { get; set; } // Unique identifier for the relationship
    public Guid FileId { get; set; } // Reference to the file
    public Guid TagId { get; set; } // Reference to the tag
}
