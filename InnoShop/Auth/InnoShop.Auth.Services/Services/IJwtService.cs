using InnoShop.Shared.Domain.Models;

public interface IJwtService
{
    string GenerateToken(User user);
    string HashPassword(string password);
    bool VerifyPassword(string password, string passwordHash);
}