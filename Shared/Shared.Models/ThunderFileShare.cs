namespace Shared.Models;

public class ThunderFileShare
{
    public Guid FileShareId { get; set; } // Unique identifier for the file share entry
    public Guid FileId { get; set; } // Reference to the file being shared
    public Guid OwnerId { get; set; } // Owner of the file (UserId from Identity)
    public Guid RecipientId { get; set; } // The user who is receiving access to the file
    public byte[] EncryptedFileKeyForRecipient { get; set; } // AES key encrypted with the recipient's public key
    public DateTime SharedAt { get; set; } // Date when the file was shared
    public FilePermission Permission { get; set; } // Permissions for the recipient
}


