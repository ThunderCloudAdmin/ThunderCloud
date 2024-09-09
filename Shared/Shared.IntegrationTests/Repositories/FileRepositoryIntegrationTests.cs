using Bogus;

public class FileRepositoryIntegrationTests : IClassFixture<TestContainersSetup>
{
    private readonly TestContainersSetup _containerSetup;
    private readonly ApplicationDbContext _context;
    private readonly FileRepository _fileRepository;

    public FileRepositoryIntegrationTests(TestContainersSetup containerSetup)
    {
        _containerSetup = containerSetup;
        _context = ApplicationDbContextFactory.Create(_containerSetup.ConnectionString);
        _fileRepository = new FileRepository(_context);
    }

    [Fact]
    public async Task AddAsync_ShouldAddFile()
    {
        // Arrange
        var file = new File { FileId = Guid.NewGuid(), FileName = "TestFile.txt", FileSize = 1234, UploadDate = DateTime.UtcNow };

        // Act
        await _fileRepository.AddAsync(file);

        // Assert
        var result = await _context.Files.FindAsync(file.FileId);
        result.Should().NotBeNull();
        result.FileName.Should().Be(file.FileName);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnFile_WhenFileExists()
    {
        // Arrange
        var fileId = Guid.NewGuid();
        var file = new File { FileId = fileId, FileName = "TestFile.txt", FileSize = 1234, UploadDate = DateTime.UtcNow };
        await _fileRepository.AddAsync(file);

        // Act
        var result = await _fileRepository.GetByIdAsync(fileId);

        // Assert
        result.Should().NotBeNull();
        result.FileName.Should().Be(file.FileName);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveFile()
    {
        // Arrange
        var fileId = Guid.NewGuid();
        var file = new File { FileId = fileId, FileName = "TestFile.txt", FileSize = 1234, UploadDate = DateTime.UtcNow };
        await _fileRepository.AddAsync(file);

        // Act
        await _fileRepository.DeleteAsync(file);

        // Assert
        var result = await _context.Files.FindAsync(fileId);
        result.Should().BeNull();
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateFile()
    {
        // Arrange
        var fileId = Guid.NewGuid();
        var file = new File { FileId = fileId, FileName = "TestFile.txt", FileSize = 1234, UploadDate = DateTime.UtcNow };
        await _fileRepository.AddAsync(file);

        file.FileName = "UpdatedFile.txt";
        await _fileRepository.UpdateAsync(file);

        // Act
        var result = await _context.Files.FindAsync(fileId);

        // Assert
        result.Should().NotBeNull();
        result.FileName.Should().Be("UpdatedFile.txt");
    }

    // Additional tests for AddFileShareAsync, GetFileShareAsync, DeleteFileShareAsync, etc.

    using System;
using System.Threading.Tasks;
using Bogus;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using Xunit;

public class FileRepositoryIntegrationTests : IClassFixture<PostgresTestContainer>
{
    private readonly ApplicationDbContext _context;
    private readonly IFileRepository _repository;
    private readonly Faker _faker = new Faker();

    public FileRepositoryIntegrationTests(PostgresTestContainer fixture)
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql(fixture.ConnectionString)
            .Options;

        _context = new ApplicationDbContext(options);
        _context.Database.EnsureCreated();

        _repository = new FileRepository(_context);
    }

    [Fact]
    public async Task AddAsync_ShouldAddFile()
    {
        // Arrange
        var file = new File
        {
            FileId = Guid.NewGuid(),
            FileName = _faker.File.FileName(),
            FileSize = _faker.Random.Int(),
            UploadDate = DateTime.UtcNow
        };

        // Act
        await _repository.AddAsync(file);
        var result = await _repository.GetByIdAsync(file.FileId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(file.FileId, result.FileId);
        Assert.Equal(file.FileName, result.FileName);
    }
}

}
