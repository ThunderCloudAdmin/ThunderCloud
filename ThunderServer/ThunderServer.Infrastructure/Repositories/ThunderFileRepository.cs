using ThunderServer.Infrastructure.Repositories.Interfaces;

namespace ThunderServer.Infrastructure.Repositories
{
    public class ThunderFileRepository : IThunderFileRepository
    {
        private readonly ThunderServerContext _dbContext;

		public ThunderFileRepository(ThunderServerContext dbContext)
		{
			_dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
		}
	}
}
