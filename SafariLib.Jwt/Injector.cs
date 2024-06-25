using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
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