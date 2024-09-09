using ThunderServer.API.Services.Interfaces;
using ThunderServer.Infrastructure.Repositories.Interfaces;

namespace ThunderServer.API.Services;

public class ThunderFileService : IThunderFileService
{
	private readonly IThunderFileRepository _repository;

	public ThunderFileService(IThunderFileRepository repository)
	{
		_repository = repository ?? throw new ArgumentNullException(nameof(repository));
	}

	public Task Create(Guid? parentFolderId, string folderToCreateName)
	{
		throw new NotImplementedException();
	}
}
