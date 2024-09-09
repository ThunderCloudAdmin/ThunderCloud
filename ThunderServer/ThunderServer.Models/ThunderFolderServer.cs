using Shared.Models;

namespace ThunderServer.Models;

public class ThunderFolderServer : ThunderFolder
{
     public virtual ThunderUser User { get; init; }
}
