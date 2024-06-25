using Microsoft.IdentityModel.Tokens;

namespace SafariLib.Jwt.Services;

public interface IJwtConfigService
{
    string CookieName { get; }
    long BearerTokenExpiration { get; }
    long RefreshTokenExpiration { get; }
    TokenValidationParameters GetTokenParameters();
    SigningCredentials GetSigningSecret();
}