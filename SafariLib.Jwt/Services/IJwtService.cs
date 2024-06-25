using SafariLib.Jwt.Models;

namespace SafariLib.Jwt.Services;

public interface IJwtService
{
    JwtToken<T> ValidateToken<T>(string? token);
    string GenerateBearerToken<T>(T content);
    string GenerateRefreshToken<T>(T content);
}