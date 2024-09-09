namespace Shared.Models
{
    public class AesKey : BaseEncryptionKey
    {
        public AesKey(byte[] key, byte[] iv) : base(key, iv)
        {
        }
    }
}
