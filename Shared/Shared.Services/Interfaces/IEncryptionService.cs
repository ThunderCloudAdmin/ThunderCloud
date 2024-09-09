using Shared.Models;

namespace Shared.Services.Interfaces;


public interface IEncryptionService<T> where T : BaseEncryptionKey
{
    byte[] Encrypt(byte[] data, T key);
    byte[] Decrypt(byte[] data, T key);
    T GenerateEncryptionKey(int keySize = 32, int ivSize = 16);
}