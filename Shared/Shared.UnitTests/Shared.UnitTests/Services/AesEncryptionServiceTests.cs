using Bogus;
using Shared.Models;
using Shared.Services;
using System.Text;

namespace Shared.UnitTests.Services;

public class AesEncryptionServiceTests : IClassFixture<AesEncryptionService>
{
    private readonly AesEncryptionService service;
    private readonly Faker faker;

    public AesEncryptionServiceTests()
    {
        this.service = new AesEncryptionService();
        this.faker = new Faker();
    }

    [Fact]
    public void Encrypt_ShouldReturnEncryptedData_WhenValidDataProvided()
    {
        // Arrange
        var testData = faker.Name.FullName();
        var key = AesKey.GenerateEncryptionKey();

        // Act
        var encryptedData = service.Encrypt(Encoding.UTF8.GetBytes(testData), key);
        var decryptedData = service.Decrypt(encryptedData, key);
        var decryptedText = Encoding.UTF8.GetString(decryptedData).TrimEnd('\0');

        // Assert
        Assert.Equal(testData, decryptedText);
    }

    [Fact]
    public void Encrypt_ShouldHandleEmptyData()
    {
        // Arrange
        var testData = string.Empty;
        var key = AesKey.GenerateEncryptionKey();

        // Act
        var encryptedData = service.Encrypt(Encoding.UTF8.GetBytes(testData), key);
        var decryptedData = service.Decrypt(encryptedData, key);
        var decryptedText = Encoding.UTF8.GetString(decryptedData).TrimEnd('\0');

        // Assert
        Assert.Equal(testData, decryptedText);
    }

    [Fact]
    public void Encrypt_ShouldHandleLargeData()
    {
        // Arrange
        var testData = new string('A', 1024); // 1 KB of data
        var key = AesKey.GenerateEncryptionKey();

        // Act
        var encryptedData = service.Encrypt(Encoding.UTF8.GetBytes(testData), key);
        var decryptedData = service.Decrypt(encryptedData, key);
        var decryptedText = Encoding.UTF8.GetString(decryptedData).TrimEnd('\0');

        // Assert
        Assert.Equal(testData, decryptedText);
    }

    [Fact]
    public void Encrypt_ShouldHandleDataExactlyOneBlockSize()
    {
        // Arrange
        var testData = new string('B', 16); // 16 bytes of data (one block for Twofish)
        var key = AesKey.GenerateEncryptionKey();

        // Act
        var encryptedData = service.Encrypt(Encoding.UTF8.GetBytes(testData), key);
        var decryptedData = service.Decrypt(encryptedData, key);
        var decryptedText = Encoding.UTF8.GetString(decryptedData).TrimEnd('\0');

        // Assert
        Assert.Equal(testData, decryptedText);
    }

    [Fact]
    public void Encrypt_ShouldHandleDataMultipleOfBlockSize()
    {
        // Arrange
        var testData = new string('C', 32); // 32 bytes of data (2 blocks for Twofish)
        var key = AesKey.GenerateEncryptionKey();

        // Act
        var encryptedData = service.Encrypt(Encoding.UTF8.GetBytes(testData), key);
        var decryptedData = service.Decrypt(encryptedData, key);
        var decryptedText = Encoding.UTF8.GetString(decryptedData).TrimEnd('\0');

        // Assert
        Assert.Equal(testData, decryptedText);
    }

    [Fact]
    public void Encrypt_ShouldReturnDifferentResultsForDifferentKeys()
    {
        // Arrange
        var testData = "Secret Data";
        var key1 = AesKey.GenerateEncryptionKey();
        var key2 = AesKey.GenerateEncryptionKey(); // Different key

        // Act
        var encryptedData1 = service.Encrypt(Encoding.UTF8.GetBytes(testData), key1);
        var encryptedData2 = service.Encrypt(Encoding.UTF8.GetBytes(testData), key2);

        // Assert
        Assert.NotEqual(Convert.ToBase64String(encryptedData1), Convert.ToBase64String(encryptedData2));
    }

    [Fact]
    public void Encrypt_ShouldHandleKeyAndIVOfDifferentSizes()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => AesKey.GenerateEncryptionKey(64, 16));
    }
}
