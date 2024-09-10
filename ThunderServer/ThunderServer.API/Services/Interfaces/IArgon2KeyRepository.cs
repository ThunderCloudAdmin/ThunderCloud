using ThunderServer.Models;

namespace ThunderServer.API.Services.Interfaces;

public interface IArgon2KeyRepository
{
    Task<Argon2Key> AddAsync(Argon2Key argonKey);
}