using System.Text;
using Microsoft.IdentityModel.Tokens;
using Moq;
using SafariLib.Core.Random;
using SafariLib.Core.Validation;
using SafariLib.Jwt.Models;
using SafariLib.Jwt.Services;

namespace SafariLib.Jwt.Tests.Services;

public class JwtServiceTest
{
    private readonly SymmetricSecurityKey _secret;
    private readonly TokenValidationParameters _tokenValidationParameters;
    private JwtService _jwtService;

    public JwtServiceTest()
    {
        _secret = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(RandomStringUtils.GenerateRandomSecret()));
        _tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false,
            ValidIssuers = ["test"],
            ValidAudiences = ["test"],
            IssuerSigningKey = _secret
        };
    }

    [Fact]
    public void ValidateToken_ReturnsExpectedResult()
    {
        // Arrange
        Setup(new JwtOptions() {
            Audience = "test",
            BearerTokenExpiration = 2000,
            CookieName = "test",
            Issuer = "test",
            RefreshTokenExpiration = 20000,
            Secret = _secret.ToString()
        });
        var content = new TokenContent { Id = 1, Name = "test" };
        var token = _jwtService.GenerateBearerToken(content);

        // Act
        var result = _jwtService.ValidateToken<TokenContent>(token);

        // Assert
        Assert.Equal(token, result.Token);
        Assert.Equal(content.Id, result.Content!.Id);
        Assert.Equal(content.Name, result.Content!.Name);
        Assert.Empty(result.Errors);
        Assert.False(result.HasError);
    }

    [Fact]
    public void ValidateToken_ReturnsError()
    {
        // Arrange
        Setup(new JwtOptions() {
            Audience = "test",
            BearerTokenExpiration = 2000,
            CookieName = "test",
            Issuer = "test",
            RefreshTokenExpiration = 20000,
            Secret = _secret.ToString()
        });
        var content = new TokenContent { Id = 1, Name = "test" };
        var token = _jwtService.GenerateBearerToken(content);

        // Act
        var result = _jwtService.ValidateToken<TokenContent>($"{token}:invalid");

        // Assert
        Assert.Collection(result.Errors, e => Assert.IsType<ResultMessage>(e));
        Assert.True(result.HasError);
    }

    [Fact]
    public void GenerateBearerToken_ReturnsExpectedResult()
    {
        // Arrange
        Setup(new JwtOptions() {
            Audience = "test",
            BearerTokenExpiration = 2000,
            CookieName = "test",
            Issuer = "test",
            RefreshTokenExpiration = 20000,
            Secret = _secret.ToString()
        });
        var content = new { test = "test" };

        // Act
        var token = _jwtService.GenerateBearerToken(content);

        // Assert
        Assert.NotNull(token);
    }

    [Fact]
    public void GenerateRefreshToken_ReturnsExpectedResult()
    {
        // Arrange
        Setup(new JwtOptions() {
            Audience = "test",
            BearerTokenExpiration = 2000,
            CookieName = "test",
            Issuer = "test",
            RefreshTokenExpiration = 20000,
            Secret = _secret.ToString()
        });
        var content = new { test = "test" };

        // Act
        var token = _jwtService.GenerateRefreshToken(content);

        // Assert
        Assert.NotNull(token);
    }

    [Fact]
    public void Config_SetsPropertiesCorrectly()
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
        Assert.Equal("test_cookie", _jwtService.GetCookieName());
        Assert.Equal(1000, _jwtService.GetBearerTokenExpiration());
        Assert.Equal(2000, _jwtService.GetRefreshTokenExpiration());
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
        var parameters = _jwtService.GetTokenParameters();

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
        var secret = _jwtService.GetSigningSecret();

        // Assert
        Assert.Equal("test_secret", Encoding.ASCII.GetString(((SymmetricSecurityKey)secret.Key).Key));
        Assert.Equal(SecurityAlgorithms.HmacSha256, secret.Algorithm);
    }

    private class TokenContent
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }

    private void Setup(JwtOptions opts) => _jwtService = new JwtService(opts);
}