using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SafariLib.Core.Enum;

namespace SafariLib.Jwt.Utils;

public static class AppSettingsUtils
{
    public static T? GetAppSetting<T>(this IConfiguration configuration, ESetting setting) =>
        configuration.GetSection(setting.GetDisplayName()).Get<T>();

    public static T? GetAppSetting<T>(this IServiceCollection services, ESetting setting)
    {
        var configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
        return configuration.GetAppSetting<T>(setting);
    }
}

public enum ESetting
{
    [Display(Name = "Jwt:RefreshExpiration")]
    JwtRefreshExpiration,

    [Display(Name = "Jwt:BearerExpiration")]
    JwtBearerExpiration,
    [Display(Name = "Jwt:Secret")] JwtSecret,
    [Display(Name = "Jwt:Issuer")] JwtIssuer,
    [Display(Name = "Jwt:Audience")] JwtAudience,
    [Display(Name = "Jwt:CookieName")] JwtCookieName,

    [Display(Name = "Jwt:MaxTokenAllowed")]
    JwtMaxTokenAllowed
}