namespace Shared.Models;

public class EncryptionKey
{
    public Guid Id { get; set; }
    public Guid FileId { get; set; }
    public string Key { get; set; }
    public DateTime CreatedDate { get; set; }

    //public ThunderFile File { get; set; }
}
