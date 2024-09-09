public class FileServiceIntegrationTests : IClassFixture<PostgresTestContainer>
{
    private readonly ApplicationDbContext _context;
    private readonly FileService _fileService;
    private readonly IFileRepository _fileRepository;
    private readonly IEncryptionKeyRepository _encryptionKeyRepository;
    private readonly IEncryptionService _encryptionService;
    private readonly IStorageProvider _storageProvider;
    private readonly Faker _faker = new Faker();

    public FileServiceIntegrationTests(PostgresTestContainer fixture)
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql(fixture.ConnectionString)
            .Options;

        _context = new ApplicationDbContext(options);
        _context.Database.EnsureCreated();

        _fileRepository = new FileRepository(_context);
        _encryptionKeyRepository = new EncryptionKeyRepository(_context);
        _encryptionService = Substitute.For<IEncryptionService>();
        _storageProvider = Substitute.For<IStorageProvider>();

        _fileService = new FileService(
            _fileRepository,
            _encryptionKeyRepository,
            _encryptionService,
            _storageProvider);
    }

    [Fact]
    public async Task UploadFileAsync_ShouldEncryptAndStoreFile()
    {
        // Arrange
        var fileId = Guid.NewGuid();
        var fileData = _faker.Lorem.Bytes(1024);
        var key = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        var encryptedData = _faker.Lorem.Bytes(1024); // Mock encrypted data
        _encryptionService.EncryptAsync(fileData, key).Returns(Task.FromResult(encryptedData));
        _storageProvider.SaveFileAsync(fileId.ToString(), encryptedData).Returns(Task.CompletedTask);

        // Act
        await _fileService.UploadFileAsync(fileId, fileData);

        // Assert
        await _storageProvider.Received(1).SaveFileAsync(fileId.ToString(), encryptedData);
        await _encryptionKeyRepository.Received(1).AddAsync(Arg.Is<EncryptionKey>(ek => ek.FileId == fileId));
    }

    [Fact]
    public async Task DownloadFileAsync_ShouldDecryptFile()
    {
        // Arrange
        var fileId = Guid.NewGuid();
        var key = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        var encryptedData = _faker.Lorem.Bytes(1024); // Mock encrypted data
        var decryptedData = _faker.Lorem.Bytes(1024); // Mock decrypted data

        _storageProvider.ReadFileAsync(fileId.ToString()).Returns(Task.FromResult(encryptedData));
        _encryptionKeyRepository.GetByFileIdAsync(fileId).Returns(new EncryptionKey { FileId = fileId, Key = key });
        _encryptionService.DecryptAsync(encryptedData, key).Returns(Task.FromResult(decryptedData));

        // Act
        var result = await _fileService.DownloadFileAsync(fileId);

        // Assert
        Assert.Equal(decryptedData, result);
        await _storageProvider.Received(1).ReadFileAsync(fileId.ToString());
    }
}
