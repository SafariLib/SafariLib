using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SafariLib.Jwt.Models;
using SafariLib.Jwt.Services;

namespace SafariLib.Jwt.Tests;

public class InjectorTest
{
    [Fact]
    public async void InjectJwtService_ShouldSuccessfullyInjectServices()
    {
        // Arrange
        var services = new ServiceCollection();
        var options = new JwtOptions
        {
            Secret = "SuperSecretKeyVeryVeryVeryVeryVeryVeryVeryLong",
            Issuer = "https://localhost:5001",
            Audience = "https://localhost:5001",
            BearerTokenExpiration = 3600,
            RefreshTokenExpiration = 604800,
            CookieName = "MdrLol"
        };

        // Act
        services.AddJwtService(options);
        var provider = services.BuildServiceProvider();
        var schemeProvider = provider.GetRequiredService<IAuthenticationSchemeProvider>();
        var scheme = await schemeProvider.GetSchemeAsync(JwtBearerDefaults.AuthenticationScheme);
        var optionsMonitor = provider.GetRequiredService<IOptionsMonitor<JwtBearerOptions>>();
        var jwtBearerOptions = optionsMonitor.Get(JwtBearerDefaults.AuthenticationScheme);
        var jwtConfig = provider.GetRequiredService<IJwtConfigService>();
        var jwtService = provider.GetRequiredService<IJwtService>();
        var authenticationService = provider.GetRequiredService<IAuthenticationService>();

        // Assert
        Assert.NotNull(jwtConfig);
        Assert.NotNull(jwtService);
        Assert.NotNull(authenticationService);
        Assert.NotNull(jwtConfig.GetTokenParameters());
        Assert.NotNull(jwtConfig.GetSigningSecret());
        Assert.Equal(options.CookieName, jwtConfig.CookieName);
        Assert.Equal(options.BearerTokenExpiration, jwtConfig.BearerTokenExpiration);
        Assert.Equal(options.RefreshTokenExpiration, jwtConfig.RefreshTokenExpiration);
        Assert.NotNull(scheme);
        Assert.Equal(JwtBearerDefaults.AuthenticationScheme, scheme.Name);
        Assert.True(jwtBearerOptions.SaveToken);
        Assert.False(jwtBearerOptions.RequireHttpsMetadata);
        Assert.NotNull(jwtBearerOptions.TokenValidationParameters);
    }
}