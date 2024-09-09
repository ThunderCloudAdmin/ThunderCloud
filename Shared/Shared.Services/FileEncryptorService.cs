using Microsoft.Extensions.Logging;
using Shared.Models;
using Shared.Services.Interfaces;

namespace Shared.Services;

public class FileEncryptorService : IFileEncryptorService
{
    private readonly IRSAKeyService rSAKeyService;
    private readonly ITwoFishEncryptionService twoFishEncryptionService;
    private readonly IAesEncryptionService aesEncryptionService;
    private readonly ILogger<FileEncryptorService> logger;

    public FileEncryptorService(IRSAKeyService rSAKeyService, ITwoFishEncryptionService twoFishEncryptionService, IAesEncryptionService aesEncryptionService, ILogger<FileEncryptorService> logger)
    {
        this.rSAKeyService = rSAKeyService ?? throw new ArgumentNullException(nameof(rSAKeyService));
        this.twoFishEncryptionService = twoFishEncryptionService ?? throw new ArgumentNullException(nameof(twoFishEncryptionService));
        this.aesEncryptionService = aesEncryptionService ?? throw new ArgumentNullException(nameof(aesEncryptionService));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public EncryptedBundle GenerateEncryptedFile(byte[] dataToEncrypt)
    {
        //Generate 2fish key
        var aesKey = TwoFishKey.GenerateEncryptionKey();

        //EncryptFileWith2FishKey
        var encryptedFile = twoFishEncryptionService.Encrypt(dataToEncrypt, aesKey);

        //Retrieve user's public key
        var rsaKeyPair = this.rSAKeyService.GenerateRsaKeyPair();

        var rsa = rsaKeyPair.LoadPublicKeyAsRSA();

        var encrypted2FishKey = this.rSAKeyService.EncryptSymmetricKeyWithRSA(aesKey.Key, rsa);

        return new EncryptedBundle(encryptedFile, encrypted2FishKey);
    }
}
