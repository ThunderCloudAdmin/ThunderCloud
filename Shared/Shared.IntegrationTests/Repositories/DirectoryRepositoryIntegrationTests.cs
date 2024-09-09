public class DirectoryRepositoryIntegrationTests : IClassFixture<TestContainersSetup>
{
    private readonly TestContainersSetup _containerSetup;
    private readonly ApplicationDbContext _context;
    private readonly DirectoryRepository _directoryRepository;

    public DirectoryRepositoryIntegrationTests(TestContainersSetup containerSetup)
    {
        _containerSetup = containerSetup;
        _context = ApplicationDbContextFactory.Create(_containerSetup.ConnectionString);
        _directoryRepository = new DirectoryRepository(_context);
    }

    [Fact]
    public async Task AddAsync_ShouldAddDirectory()
    {
        // Arrange
        var directory = new Directory { DirectoryId = Guid.NewGuid(), DirectoryName = "TestDirectory", UserId = Guid.NewGuid() };

        // Act
        await _directoryRepository.AddAsync(directory);

        // Assert
        var result = await _context.Directories.FindAsync(directory.DirectoryId);
        result.Should().NotBeNull();
        result.DirectoryName.Should().Be(directory.DirectoryName);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnDirectory_WhenExists()
    {
        // Arrange
        var directoryId = Guid.NewGuid();
        var directory = new Directory { DirectoryId = directoryId, DirectoryName = "TestDirectory", UserId = Guid.NewGuid() };
        await _directoryRepository.AddAsync(directory);

        // Act
        var result = await _directoryRepository.GetByIdAsync(directoryId);

        // Assert
        result.Should().NotBeNull();
        result.DirectoryName.Should().Be(directory.DirectoryName);
    }

    // Additional tests for GetDirectoriesByUserAsync, etc.
}
