using Bogus;
using ThunderServer.Models;

namespace ThunderServer.UnitTests;

public class Argon2KeyTests
{
    private readonly Faker _faker;

    public Argon2KeyTests()
    {
        _faker = new Faker();
    }

    [Fact]
    public void DeriveKeyFromPassword_ShouldGenerateKeyWithCorrectProperties()
    {
        // Arrange
        var password = _faker.Internet.Password(); // Generate a random password using Bogus
        var saltSize = 16;
        var keyLength = 32;
        // Act
        var argon2Key = Argon2Key.DeriveKeyFromPassword(password, saltSize, keyLength);

        // Assert
        Assert.NotNull(argon2Key);
        Assert.NotNull(argon2Key.Salt);
        Assert.Equal(saltSize, argon2Key.Salt.Length);
        Assert.NotNull(argon2Key.DerivedKey);
        Assert.Equal(keyLength, argon2Key.DerivedKey.Length);
        Assert.Equal(4, argon2Key.Iterations);
        Assert.Equal(65536, argon2Key.MemorySize);
        Assert.Equal(8, argon2Key.Parallelism);
    }
}
