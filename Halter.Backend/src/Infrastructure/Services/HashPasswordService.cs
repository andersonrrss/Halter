using System.Security.Cryptography;
using System.Text;
using GymApp.Application.Interfaces;
using Konscious.Security.Cryptography;

namespace GymApp.Infrastructure.Services;

public class HashPasswordService : IHashPasswordService
{
    public async Task<string> HashAsync(string password)
    {
        var salt = GenerateRandomSalt(16);
        var hash = await ComputeHashAsync(password, salt);

        string saltString = Convert.ToBase64String(salt);
        string hashString = Convert.ToBase64String(hash);

        return $"{saltString}:{hashString}";
    }

    public async Task<bool> VerifyAsync(string password, string storedHash)
    {
        var parts = storedHash.Split(":");

        byte[] salt = Convert.FromBase64String(parts[0]);
        byte[] hash = Convert.FromBase64String(parts[1]);

        var computedHash = await ComputeHashAsync(password, salt);

        return CryptographicOperations.FixedTimeEquals(computedHash, hash);
    }

    private async Task<byte[]> ComputeHashAsync(string password, byte[] salt)
    {
        using var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password)){
            Salt = salt,
            DegreeOfParallelism = 4,
            MemorySize = 65536,
            Iterations = 4
        };

        return await argon2.GetBytesAsync(32);
    }

    private byte[] GenerateRandomSalt(int length)
    {
        var salt = new byte[length];
        RandomNumberGenerator.Fill(salt);
        return salt;
    }
}
