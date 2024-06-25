using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.IdentityModel.Tokens;
using SafariLib.Jwt.Models;

namespace SafariLib.Jwt.Services;

public class JwtService(JwtOptions options) : IJwtService
{
    
    private readonly TokenValidationParameters _tokenValidationParameters = new()
    {
        ValidateIssuerSigningKey = true,
        ValidIssuer = options.Issuer,
        ValidAudience = options.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(options.Secret!)),
        ClockSkew = TimeSpan.Zero
    };

    private const string ContentClaimType = "Content";
    public string GetCookieName() => options.CookieName;
    public long GetBearerTokenExpiration() => options.BearerTokenExpiration;
    public long GetRefreshTokenExpiration() => options.RefreshTokenExpiration;

    public TokenValidationParameters GetTokenParameters() => _tokenValidationParameters;

    public SigningCredentials GetSigningSecret() =>
        new(_tokenValidationParameters.IssuerSigningKey, SecurityAlgorithms.HmacSha256);


    public JwtToken<T> ValidateToken<T>(string? token)
    {
        var result = new JwtToken<T> { Token = token };
        var handler = new JwtSecurityTokenHandler();

        try
        {
            handler.ValidateToken(token, GetTokenParameters(), out var validatedToken);
            var content = handler
                .ReadJwtToken(token)
                .Claims.First(c => c.Type == ContentClaimType)
                .Value;
            result.Content = JsonSerializer.Deserialize<T>(content);
            result.SecurityToken = validatedToken;
        }
        catch (Exception e)
        {
            result.AddError(e);
        }

        return result;
    }

    public string GenerateBearerToken<T>(T content) =>
        SignToken(content, DateTime.UtcNow.AddMilliseconds(options.BearerTokenExpiration));

    public string GenerateRefreshToken<T>(T content) =>
        SignToken(content, DateTime.UtcNow.AddMilliseconds(options.RefreshTokenExpiration));

    private string SignToken<T>(T obj, DateTime expires)
    {
        var claims = new List<Claim> { new(ContentClaimType, JsonSerializer.Serialize(obj)) };
        var parameters = GetTokenParameters();
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(
            new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expires,
                SigningCredentials = GetSigningSecret(),
                Issuer = parameters.ValidIssuer,
                Audience = parameters.ValidAudience
            }
        );
        return tokenHandler.WriteToken(token);
    }
}