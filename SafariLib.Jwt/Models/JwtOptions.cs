namespace SafariLib.Jwt.Models;

public class JwtOptions
{
    public string? Audience;
    public long BearerTokenExpiration;
    public string? CookieName;
    public string? Issuer;
    public int? MaxTokenAllowed;
    public long RefreshTokenExpiration;
    public string? Secret;
}