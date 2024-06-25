using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using SafariLib.Core.Random;
using SafariLib.Jwt.Cache;
using SafariLib.Jwt.Models;
using SafariLib.Jwt.Services;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SafariLib.Jwt;

public static class Injector
{
    public static IServiceCollection AddJwtService(
        this IServiceCollection services,
        JwtOptions options
    )
    {
        options = ValidateOptions(options);
        var JwtService = new JwtService(options);
        services.AddScoped<IJwtService, JwtService>(_ => JwtService);

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(bearerOptions =>
            {
                bearerOptions.SaveToken = true;
                bearerOptions.RequireHttpsMetadata = false;
                bearerOptions.TokenValidationParameters = JwtService.GetTokenParameters();
            });

        return services;
    }

    private static JwtOptions ValidateOptions(JwtOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.Audience))
            throw new ArgumentNullException(nameof(options.Audience));
        if (string.IsNullOrWhiteSpace(options.Issuer))
            throw new ArgumentNullException(nameof(options.Issuer));
        if (string.IsNullOrWhiteSpace(options.CookieName))
            options.CookieName = "RefreshToken";
        if (string.IsNullOrWhiteSpace(options.Secret))
            options.Secret = RandomStringUtils.GenerateRandomSecret();
        if (options.BearerTokenExpiration == 0)
            throw new ArgumentNullException(nameof(options.BearerTokenExpiration));
        if (options.RefreshTokenExpiration == 0)
            throw new ArgumentNullException(nameof(options.RefreshTokenExpiration));
        return options;
    }

    public static IServiceCollection AddJwtCacheService(this IServiceCollection services, int? maxTokenAllowed = null)
        => services.AddScoped<IJwtCacheService, JwtCacheService>(
            _ => new JwtCacheService(_.GetRequiredService<IMemoryCache>(),maxTokenAllowed ?? 1)
            );

    public static void AddJwtSwagger(this SwaggerGenOptions options)
    {
        options.AddSecurityDefinition(
            "Bearer",
            new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme",
                Name = "Authorization",
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey
            }
        );
        options.AddSecurityRequirement(
            new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "Bearer"
                    },
                    Array.Empty<string>()
                }
            }
        );
    }
}