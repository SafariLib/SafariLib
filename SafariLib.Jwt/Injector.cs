using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using SafariLib.Core.Random;
using SafariLib.Jwt.Cache;
using SafariLib.Jwt.Models;
using SafariLib.Jwt.Services;
using SafariLib.Jwt.Utils;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SafariLib.Jwt;

public static class Injector
{
    public static IServiceCollection AddJwtService(
        this IServiceCollection services,
        JwtOptions options
    )
    {
        options = ResolveOptions(services, options);

        services
            .AddScoped<IJwtConfigService, JwtConfigService>(_ => new JwtConfigService(options))
            .AddScoped<IJwtService, JwtService>();

        var jwtConfig = services.BuildServiceProvider().GetRequiredService<IJwtConfigService>();
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(bearerOptions =>
            {
                bearerOptions.SaveToken = true;
                bearerOptions.RequireHttpsMetadata = false;
                bearerOptions.TokenValidationParameters = jwtConfig.GetTokenParameters();
            });

        return services;
    }

    public static IServiceCollection AddJwtCacheService(this IServiceCollection services)
        => services.AddScoped<IJwtCacheService, JwtCacheService>();

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

    private static JwtOptions ResolveOptions(IServiceCollection services, JwtOptions options)
    {
        if (options.MaxTokenAllowed == 0)
        {
            options.MaxTokenAllowed = services.GetAppSetting<int>(ESetting.JwtMaxTokenAllowed);
            options.MaxTokenAllowed = options.MaxTokenAllowed == 0 ? 5 : options.MaxTokenAllowed;
        }

        if (options.BearerTokenExpiration == 0)
        {
            options.BearerTokenExpiration = services.GetAppSetting<long>(ESetting.JwtBearerExpiration);
            options.BearerTokenExpiration =
                options.BearerTokenExpiration == 0 ? 1800000 : options.BearerTokenExpiration;
        }

        if (options.RefreshTokenExpiration == 0)
        {
            options.RefreshTokenExpiration = services.GetAppSetting<long>(ESetting.JwtRefreshExpiration);
            options.RefreshTokenExpiration =
                options.BearerTokenExpiration == 0 ? 300000 : options.BearerTokenExpiration;
        }

        options.CookieName ??= services.GetAppSetting<string>(ESetting.JwtCookieName) ?? "RefreshToken";
        options.Issuer ??= services.GetAppSetting<string>(ESetting.JwtIssuer) ?? "issuer";
        options.Audience ??= services.GetAppSetting<string>(ESetting.JwtAudience) ?? "audience";
        options.Issuer ??= services.GetAppSetting<string>(ESetting.JwtIssuer) ?? "issuer";
        options.Secret ??= services.GetAppSetting<string>(ESetting.JwtSecret) ??
                           RandomStringUtils.GenerateRandomSecret();
        return options;
    }
}