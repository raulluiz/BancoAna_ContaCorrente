using System.Security.Cryptography;
using System.Text;

namespace Application.Services;

public static class SenhaHasher
{
    public static (string hash, string salt) HashSenha(string senha)
    {
        var saltBytes = RandomNumberGenerator.GetBytes(16);
        var salt = Convert.ToBase64String(saltBytes);
        var hash = Hash(senha, salt);
        return (hash, salt);
    }

    private static string Hash(string senha, string salt)
    {
        var combined = Encoding.UTF8.GetBytes(senha + salt);
        using var sha256 = SHA256.Create();
        var hashBytes = sha256.ComputeHash(combined);
        return Convert.ToBase64String(hashBytes);
    }
}
