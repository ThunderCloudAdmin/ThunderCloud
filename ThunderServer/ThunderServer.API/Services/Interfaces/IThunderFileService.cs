namespace ThunderServer.API.Services.Interfaces
{
	public interface IThunderFileService
	{
		Task Create(Guid? parentFolderId, string folderToCreateName);
    }
}
