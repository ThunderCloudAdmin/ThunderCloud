using Bogus;
using FluentAssertions;
using NSubstitute;

public class FileServiceTests
{
    private readonly IFileRepository _fileRepository;
    private readonly IEncryptionService _encryptionService;
    private readonly IStorageProvider _storageProvider;
    private readonly FileService _fileService;

    public FileServiceTests()
    {
        _fileRepository = Substitute.For<IFileRepository>();
        _encryptionService = Substitute.For<IEncryptionService>();
        _storageProvider = Substitute.For<IStorageProvider>();
        _fileService = new FileService(_fileRepository, _encryptionService, _storageProvider);
    }

    [Fact]
    public async Task UploadFileAsync_ShouldReturnFileId_WhenFileIsUploaded()
    {
        // Arrange
        var fileId = Guid.NewGuid();
        var fakeFileStream = new MemoryStream();
        var fakeEncryptedStream = new MemoryStream();
        var request = new UploadFileRequest
        {
            File = new FormFile(fakeFileStream, 0, fakeFileStream.Length, "file", "testfile.txt"),
            FileName = "testfile.txt",
            UserId = Guid.NewGuid()
        };

        _encryptionService.Encrypt(fakeFileStream).Returns(fakeEncryptedStream);
        _storageProvider.SaveFileAsync(fileId.ToString(), fakeEncryptedStream).Returns(Task.CompletedTask);
        _fileRepository.AddAsync(Arg.Any<File>()).Returns(Task.CompletedTask);

        // Act
        var result = await _fileService.UploadFileAsync(request);

        // Assert
        result.Should().Be(fileId);
    }

    [Fact]
    public async Task GetFileAsync_ShouldReturnFileStream_WhenFileIsFound()
    {
        // Arrange
        var fileId = Guid.NewGuid();
        var fakeFileStream = new MemoryStream();
        var fakeDecryptedStream = new MemoryStream();
        var file = new File
        {
            FileId = fileId,
            FileName = "testfile.txt"
        };

        _fileRepository.GetByIdAsync(fileId).Returns(file);
        _storageProvider.GetFileAsync(fileId.ToString()).Returns(fakeFileStream);
        _encryptionService.Decrypt(fakeFileStream).Returns(fakeDecryptedStream);

        // Act
        var result = await _fileService.GetFileAsync(fileId);

        // Assert
        result.FileStream.Should().Be(fakeDecryptedStream);
        result.FileName.Should().Be(file.FileName);
    }

    [Fact]
    public async Task DeleteFileAsync_ShouldRemoveFile_WhenFileExists()
    {
        // Arrange
        var fileId = Guid.NewGuid();
        var file = new File
        {
            FileId = fileId
        };

        _fileRepository.GetByIdAsync(fileId).Returns(file);
        _fileRepository.DeleteAsync(file).Returns(Task.CompletedTask);
        _storageProvider.DeleteFileAsync(fileId.ToString()).Returns(Task.CompletedTask);

        // Act
        await _fileService.DeleteFileAsync(fileId);

        // Assert
        await _fileRepository.Received(1).DeleteAsync(file);
        await _storageProvider.Received(1).DeleteFileAsync(fileId.ToString());
    }

    [Fact]
    public async Task GetUserFilesAsync_ShouldReturnFilesForUser()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var files = new Faker<FileDto>()
            .RuleFor(f => f.FileId, f => Guid.NewGuid())
            .RuleFor(f => f.FileName, f => f.Lorem.Word())
            .RuleFor(f => f.FileSize, f => f.Random.Long(1, 1000))
            .RuleFor(f => f.UploadDate, f => f.Date.Past())
            .Generate(5);

        _fileRepository.GetFilesByUserAsync(userId).Returns(Task.FromResult(files));

        // Act
        var result = await _fileService.GetUserFilesAsync(userId);

        // Assert
        result.Should().BeEquivalentTo(files);
    }

    [Fact]
    public async Task ShareFileAsync_ShouldShareFile_WhenFileAndUserExist()
    {
        // Arrange
        var fileId = Guid.NewGuid();
        var recipientId = Guid.NewGuid();
        var permission = FilePermission.Read;

        var file = new File { FileId = fileId };
        _fileRepository.GetByIdAsync(fileId).Returns(file);

        // Act
        await _fileService.ShareFileAsync(fileId, recipientId, permission);

        // Assert
        await _fileRepository.Received(1).AddFileShareAsync(Arg.Is<FileShare>(fs =>
            fs.FileId == fileId && fs.UserId == recipientId && fs.Permission == permission));
    }

    [Fact]
    public async Task RevokeFileAccessAsync_ShouldRevokeAccess_WhenFileShareExists()
    {
        // Arrange
        var fileId = Guid.NewGuid();
        var recipientId = Guid.NewGuid();
        var fileShare = new FileShare { FileId = fileId, UserId = recipientId };

        _fileRepository.GetFileShareAsync(fileId, recipientId).Returns(fileShare);

        // Act
        await _fileService.RevokeFileAccessAsync(fileId, recipientId);

        // Assert
        await _fileRepository.Received(1).DeleteFileShareAsync(fileShare);
    }

    [Fact]
    public async Task GetSharedFilesForUserAsync_ShouldReturnSharedFiles()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var files = new Faker<FileDto>()
            .RuleFor(f => f.FileId, f => Guid.NewGuid())
            .RuleFor(f => f.FileName, f => f.Lorem.Word())
            .RuleFor(f => f.FileSize, f => f.Random.Long(1, 1000))
            .RuleFor(f => f.UploadDate, f => f.Date.Past())
            .Generate(5);

        _fileRepository.GetSharedFilesByUserAsync(userId).Returns(Task.FromResult(files));

        // Act
        var result = await _fileService.GetSharedFilesForUserAsync(userId);

        // Assert
        result.Should().BeEquivalentTo(files);
    }
}
