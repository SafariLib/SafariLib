using System.Text;
using Microsoft.IdentityModel.Tokens;
using SafariLib.Jwt.Models;
using SafariLib.Jwt.Services;

namespace SafariLib.Jwt.Tests.Services;

public class JwtConfigServiceTest
{
    private JwtConfigService _jwtConfigService;

    [Fact]
    public void JwtConfig_SetsPropertiesCorrectly()
    {
        // Arrange
        Setup(new JwtOptions
        {
            Issuer = "test_issuer",
            Audience = "test_audience",
            Secret = "test_secret",
            CookieName = "test_cookie",
            BearerTokenExpiration = 1000,
            RefreshTokenExpiration = 2000
        });

        // Assert
        Assert.Equal("test_cookie", _jwtConfigService.CookieName);
        Assert.Equal(1000, _jwtConfigService.BearerTokenExpiration);
        Assert.Equal(2000, _jwtConfigService.RefreshTokenExpiration);
    }

    [Fact]
    public void GetTokenParameters_ReturnsCorrectParameters()
    {
        // Arrange
        Setup(new JwtOptions
        {
            Issuer = "test_issuer",
            Audience = "test_audience",
            Secret = "test_secret"
        });

        // Act
        var parameters = _jwtConfigService.GetTokenParameters();

        // Assert
        Assert.True(parameters.ValidateIssuer);
        Assert.True(parameters.ValidateAudience);
        Assert.True(parameters.ValidateLifetime);
        Assert.True(parameters.ValidateIssuerSigningKey);
        Assert.Equal("test_issuer", parameters.ValidIssuer);
        Assert.Equal("test_audience", parameters.ValidAudience);
        Assert.Equal("test_secret", Encoding.ASCII.GetString(((SymmetricSecurityKey)parameters.IssuerSigningKey).Key));
        Assert.Equal(TimeSpan.Zero, parameters.ClockSkew);
    }

    [Fact]
    public void GetSigningSecret_ReturnsCorrectSecret()
    {
        // Arrange
        Setup(new JwtOptions { Secret = "test_secret" });

        // Act
        var secret = _jwtConfigService.GetSigningSecret();

        // Assert
        Assert.Equal("test_secret", Encoding.ASCII.GetString(((SymmetricSecurityKey)secret.Key).Key));
        Assert.Equal(SecurityAlgorithms.HmacSha256, secret.Algorithm);
    }

    private void Setup(JwtOptions opts) => _jwtConfigService = new JwtConfigService(opts);
}