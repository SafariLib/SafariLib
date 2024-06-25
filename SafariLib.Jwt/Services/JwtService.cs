using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.IdentityModel.Tokens;
using SafariLib.Jwt.Models;

namespace SafariLib.Jwt.Services;

public class JwtService(IJwtConfigService jwtConfig) : IJwtService
{
    private const string ContentClaimType = "Content";

    public JwtToken<T> ValidateToken<T>(string? token)
    {
        var result = new JwtToken<T> { Token = token };
        var handler = new JwtSecurityTokenHandler();

        try
        {
            handler.ValidateToken(token, jwtConfig.GetTokenParameters(), out var validatedToken);
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
        SignToken(content, DateTime.UtcNow.AddMilliseconds(jwtConfig.BearerTokenExpiration));

    public string GenerateRefreshToken<T>(T content) =>
        SignToken(content, DateTime.UtcNow.AddMilliseconds(jwtConfig.RefreshTokenExpiration));

    private string SignToken<T>(T obj, DateTime expires)
    {
        var claims = new List<Claim> { new(ContentClaimType, JsonSerializer.Serialize(obj)) };
        var parameters = jwtConfig.GetTokenParameters();
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(
            new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expires,
                SigningCredentials = jwtConfig.GetSigningSecret(),
                Issuer = parameters.ValidIssuer,
                Audience = parameters.ValidAudience
            }
        );
        return tokenHandler.WriteToken(token);
    }
}