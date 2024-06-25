using System.Text;
using Microsoft.IdentityModel.Tokens;
using Moq;
using SafariLib.Core.Random;
using SafariLib.Core.Validation;
using SafariLib.Jwt.Services;

namespace SafariLib.Jwt.Tests.Services;

public class JwtServiceTest
{
    private readonly Mock<IJwtConfigService> _jwtConfigService;
    private readonly JwtService _jwtService;
    private readonly SymmetricSecurityKey _secret;
    private readonly TokenValidationParameters _tokenValidationParameters;

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
        _jwtConfigService = new Mock<IJwtConfigService>();
        _jwtService = new JwtService(_jwtConfigService.Object);
    }

    [Fact]
    public void ValidateToken_ReturnsExpectedResult()
    {
        // Arrange
        Setup();
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
        Setup();
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
        Setup();
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
        Setup();
        var content = new { test = "test" };

        // Act
        var token = _jwtService.GenerateRefreshToken(content);

        // Assert
        Assert.NotNull(token);
    }

    private void Setup()
    {
        _jwtConfigService.Setup(c => c.BearerTokenExpiration).Returns(2000);
        _jwtConfigService.Setup(c => c.RefreshTokenExpiration).Returns(20000);
        _jwtConfigService.Setup(c => c.GetTokenParameters()).Returns(_tokenValidationParameters);
        _jwtConfigService.Setup(c => c.GetSigningSecret())
            .Returns(new SigningCredentials(_secret, SecurityAlgorithms.HmacSha256));
    }

    private class TokenContent
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }
}