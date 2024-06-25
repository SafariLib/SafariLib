using System.Text;
using Microsoft.IdentityModel.Tokens;
using SafariLib.Jwt.Models;

namespace SafariLib.Jwt.Services;

public class JwtConfigService(JwtOptions opts) : IJwtConfigService
{
    private readonly TokenValidationParameters _tokenValidationParameters = new()
    {
        ValidateIssuerSigningKey = true,
        ValidIssuer = opts.Issuer,
        ValidAudience = opts.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(opts.Secret!)),
        ClockSkew = TimeSpan.Zero
    };

    public string CookieName { get; } = opts.CookieName!;
    public long BearerTokenExpiration { get; } = opts.BearerTokenExpiration;
    public long RefreshTokenExpiration { get; } = opts.RefreshTokenExpiration;
    public TokenValidationParameters GetTokenParameters() => _tokenValidationParameters;

    public SigningCredentials GetSigningSecret() =>
        new(_tokenValidationParameters.IssuerSigningKey, SecurityAlgorithms.HmacSha256);
}