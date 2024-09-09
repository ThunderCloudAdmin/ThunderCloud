using Bogus;
using Shared.Models;
using Shared.Services;
using System.Text;

namespace Shared.UnitTests.Services;

public class TwoFishEncryptionServiceTests : IClassFixture<TwoFishEncryptionService>
{
    private readonly TwoFishEncryptionService service;
    private readonly Faker faker;

    public TwoFishEncryptionServiceTests()
    {
        this.service = new TwoFishEncryptionService();
        this.faker = new Faker();
    }

    [Fact]
    public void Encrypt_ShouldReturnEncryptedData_WhenValidDataProvided()
    {
        // Arrange
        var testData = "Hello World";
        var key = TwoFishKey.GenerateEncryptionKey();

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
        var key = TwoFishKey.GenerateEncryptionKey();

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
        var key = TwoFishKey.GenerateEncryptionKey();

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
        var key = TwoFishKey.GenerateEncryptionKey();

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
        var key = TwoFishKey.GenerateEncryptionKey();

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
        var key1 = TwoFishKey.GenerateEncryptionKey(32, 16);
        var key2 = TwoFishKey.GenerateEncryptionKey(32, 16); // Different key

        // Act
        var encryptedData1 = service.Encrypt(Encoding.UTF8.GetBytes(testData), key1);
        var encryptedData2 = service.Encrypt(Encoding.UTF8.GetBytes(testData), key2);

        // Assert
        Assert.NotEqual(Convert.ToBase64String(encryptedData1), Convert.ToBase64String(encryptedData2));
    }

    [Fact]
    public void GenerateEncryptionKey_ShouldHandleKeyAndIVOfDifferentSizes()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => TwoFishKey.GenerateEncryptionKey(16, 16));
    }
}
