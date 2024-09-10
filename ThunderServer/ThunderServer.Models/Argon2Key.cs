using Konscious.Security.Cryptography;
using System.Security.Cryptography;
using System.Text;

namespace ThunderServer.Models;

public class Argon2Key
{
    public byte[] Salt { get; private set; }

    public byte[] DerivedKey { get; private set; }

    public Guid Id { get; private set; }

    public int Iterations { get; private set; } = 4;

    public int MemorySize { get; private set; } = 65536;

    public int Parallelism { get; private set; } = 8;

    public RsaKeyPairServer RsaKeyPairServer { get; private set; }

    protected Argon2Key() { }

    private Argon2Key(int iterations = 4, int memorySize = 65536, int parallelism = 8)
    {
        Iterations = iterations;
        MemorySize = memorySize;
        Parallelism = parallelism;
    }

    public static Argon2Key DeriveKeyFromPassword(string password, int saltSize = 16, int keyLength = 32)
    {
        Argon2Key argon2Key = new Argon2Key();

        var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
        {
            Salt = GenerateSalt(saltSize),
            DegreeOfParallelism = argon2Key.Parallelism,  // Parallelism factor
            MemorySize = argon2Key.MemorySize,       // Memory cost in KB (64 MB)
            Iterations = argon2Key.Iterations           // Time cost (iterations)
        };

        // Derive a 256-bit key for AES-256 encryption

        argon2Key.DerivedKey = argon2.GetBytes(keyLength);
        argon2Key.Salt = argon2.Salt;
        return argon2Key;
    }

    private static byte[] GenerateSalt(int size = 16)
    {
        return RandomNumberGenerator.GetBytes(size);
    }
}
