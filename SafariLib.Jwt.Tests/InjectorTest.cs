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
        var jwtService = provider.GetRequiredService<IJwtService>();
        var authenticationService = provider.GetRequiredService<IAuthenticationService>();

        // Assert
        Assert.NotNull(jwtService);
        Assert.NotNull(authenticationService);
        Assert.NotNull(jwtService.GetTokenParameters());
        Assert.NotNull(jwtService.GetSigningSecret());
        Assert.Equal(options.CookieName, jwtService.GetCookieName());
        Assert.Equal(options.BearerTokenExpiration, jwtService.GetBearerTokenExpiration());
        Assert.Equal(options.RefreshTokenExpiration, jwtService.GetRefreshTokenExpiration());
        Assert.NotNull(scheme);
        Assert.Equal(JwtBearerDefaults.AuthenticationScheme, scheme.Name);
        Assert.True(jwtBearerOptions.SaveToken);
        Assert.False(jwtBearerOptions.RequireHttpsMetadata);
        Assert.NotNull(jwtBearerOptions.TokenValidationParameters);
    }
}