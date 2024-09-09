public class EncryptionKeyRepositoryIntegrationTests : IClassFixture<PostgresTestContainer>
{
    private readonly ApplicationDbContext _context;
    private readonly IEncryptionKeyRepository _repository;
    private readonly Faker _faker = new Faker();

    public EncryptionKeyRepositoryIntegrationTests(PostgresTestContainer fixture)
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql(fixture.ConnectionString)
            .Options;

        _context = new ApplicationDbContext(options);
        _context.Database.EnsureCreated();

        _repository = new EncryptionKeyRepository(_context);
    }

    [Fact]
    public async Task AddAsync_ShouldAddEncryptionKey()
    {
        // Arrange
        var fileId = Guid.NewGuid();
        var key = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        var encryptionKey = new EncryptionKey
        {
            Id = Guid.NewGuid(),
            FileId = fileId,
            Key = key,
            CreatedDate = DateTime.UtcNow
        };

        // Act
        await _repository.AddAsync(encryptionKey);
        var result = await _repository.GetByFileIdAsync(fileId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(fileId, result.FileId);
        Assert.Equal(key, result.Key);
    }
}
