namespace SafariLib.Jwt.Models;

public class JwtOptions
{
    public string Audience = string.Empty;
    public long BearerTokenExpiration;
    public string CookieName = string.Empty;
    public string Issuer = string.Empty;
    public long RefreshTokenExpiration;
    public string Secret = string.Empty;
}