using Bogus;
using NSubstitute;
using System.Security.Cryptography;
using System.Text;
using ThunderServer.API.Services;
using ThunderServer.Infrastructure.Repositories.Interfaces;
using ThunderServer.Models;

namespace ThunderServer.UnitTests.Services;

public class RsaKeyEncryptionServiceTests
{
    private readonly IRsaKeyPairServerRepository _mockRepository;
    private readonly RsaKeyEncryptionService _rsaKeyEncryptionService;

    public RsaKeyEncryptionServiceTests()
    {
        // Create a substitute for the IRsaKeyPairServerRepository
        _mockRepository = Substitute.For<IRsaKeyPairServerRepository>();

        // Instantiate the RsaKeyEncryptionService with the mock repository
        _rsaKeyEncryptionService = new RsaKeyEncryptionService(_mockRepository);

        // Configure Bogus to generate fake RsaKeyPairServer
        _rsaKeyPairFaker = new Faker<RsaKeyPairServer>()
            .RuleFor(r => r.PrivateKey, f => Convert.ToBase64String(f.Random.Bytes(256)))  // Fake private key
            .RuleFor(r => r.PublicKey, f => Convert.ToBase64String(f.Random.Bytes(256)))   // Fake public key
            .RuleFor(r => r.EncryptedPrivateRsaKey, f => f.Random.Bytes(256));              // Fake encrypted private key
    }

    [Fact]
    public async Task EncryptPrivateKeyWithArgon2Key_Should_Encrypt_And_Store_RsaKeyPair()
    {
        // Arrange
        var argon2Key = Argon2Key.DeriveKeyFromPassword("MySecureArgon2Key123");

        var user = new ThunderUser()
        {
            Id = Guid.NewGuid()
        };

        // Act
        await _rsaKeyEncryptionService.EncryptPrivateKeyWithArgon2Key(user, argon2Key);

        // Assert
        await _mockRepository.Received(1).AddAsync(Arg.Is<RsaKeyPairServer>(rkp => rkp.EncryptedPrivateRsaKey != null));
    }

    [Fact]
    public async Task DecryptPrivateKeyWithArgon2Key_Should_Return_Decrypted_Key()
    {
        // Arrange
        var userGuid = Guid.NewGuid();
        var argon2Key = Encoding.UTF8.GetBytes("MySecureArgon2Key123");
        var privateKey = Encoding.UTF8.GetBytes("MyPrivateKey");

        // Act
        var decryptedPrivateKey = await _rsaKeyEncryptionService.DecryptPrivateKeyWithArgon2Key(userGuid);

        // Assert
        Assert.Equal(privateKey, decryptedPrivateKey);  // Check if decrypted private key matches
    }

    private byte[] EncryptPrivateKey(byte[] privateKey, byte[] argon2Key)
    {
        using (var aes = Aes.Create())
        {
            aes.Key = argon2Key;
            aes.GenerateIV();

            using (var encryptor = aes.CreateEncryptor())
            using (var ms = new MemoryStream())
            {
                ms.Write(aes.IV, 0, aes.IV.Length);
                using (var cryptoStream = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(privateKey, 0, privateKey.Length);
                }

                return ms.ToArray();  // Return encrypted private key (with IV)
            }
        }
    }
}
