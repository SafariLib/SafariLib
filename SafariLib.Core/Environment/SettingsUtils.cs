using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SafariLib.Core.Enum;

namespace SafariLib.Core.Environment;

public static class SettingsUtils
{
    /// <summary>
    ///     Add project settings to the configuration builder.
    ///     The settings are loaded from <strong>appsettings.json</strong>, <strong>appsettings.{env}.json</strong> and environment variables.
    ///     The environment is set by the <strong>ASPNETCORE_ENVIRONMENT</strong> environment variable.
    /// </summary>
    public static IConfigurationBuilder AddProjectSettings(this IConfigurationBuilder builder)
    {
        var env = EnvUtils.Env;
        return builder
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();
    }

    /// <summary>
    ///     Load project settings to the web application builder.
    ///     The settings are loaded from <strong>appsettings.json</strong>, <strong>appsettings.{env}.json</strong> and environment variables.
    ///     The environment is set by the <strong>ASPNETCORE_ENVIRONMENT</strong> environment variable.
    /// </summary>
    public static WebApplicationBuilder AddProjectSettings(this WebApplicationBuilder builder)
    {
        builder.Configuration.AddProjectSettings();
        return builder;
    }

    /// <summary>
    ///    Load project settings to the web application builder.
    ///    The settings are loaded from another project.
    /// </summary>
    public static IConfigurationBuilder LoadProjectSettings(this IConfigurationBuilder builder, string projectName)
        => builder.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "..", projectName))
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{EnvUtils.Env}.json");

    /// <summary>
    ///    Get a setting by name from the configuration.
    ///    The setting is loaded from <strong>appsettings.json</strong>, <strong>appsettings.{env}.json</strong> and environment variables.
    /// </summary>
    public static T? GetSetting<T>(this IConfiguration configuration, string key) 
    {
        if (typeof(T) != typeof(string[]))
            return configuration.GetSection(key).Get<T>();
        
        var value = configuration.GetSection(key).Value;
        if (string.IsNullOrEmpty(value)) return default;
        var stringArray = value.Split(',', StringSplitOptions.RemoveEmptyEntries);
        return (T)(object)stringArray;
    }

    /// <summary>
    ///   Get a setting by enum from the configuration.
    ///   The setting is loaded from <strong>appsettings.json</strong>, <strong>appsettings.{env}.json</strong> and environment variables.
    ///   The enum value is converted to a string using the Display attribute.
    ///   The setting key is the display name of the enum value.
    /// </summary>
    public static T? GetSetting<T>(this IConfiguration configuration, System.Enum setting)
    {
        var key = setting.GetDisplayName();
        return configuration.GetSetting<T>(key);
    }

    /// <summary>
    ///   Get a setting by name from the configuration.
    ///   The setting is loaded from <strong>appsettings.json</strong>, <strong>appsettings.{env}.json</strong> and environment variables.
    ///   If the setting is not found, an exception is thrown.
    /// </summary>
    public static T GetSettingOrThrow<T>(this IConfiguration configuration, string key) 
        => configuration.GetSetting<T>(key) ?? throw new Exception($"Setting {key} not found");

    /// <summary>
    ///  Get a setting by enum from the configuration.
    ///  The setting is loaded from <strong>appsettings.json</strong>, <strong>appsettings.{env}.json</strong> and environment variables.
    ///  The enum value is converted to a string using the Display attribute.
    ///  The setting key is the display name of the enum value.
    ///  If the setting is not found, an exception is thrown.
    ///  </summary>
    public static T GetSettingOrThrow<T>(this IConfiguration configuration, System.Enum setting) 
        => configuration.GetSetting<T>(setting) ?? throw new Exception($"Setting {setting.GetDisplayName()} not found");

    /// <summary>
    ///    Get a setting by name from the configuration.
    ///    The setting is loaded from <strong>appsettings.json</strong>, <strong>appsettings.{env}.json</strong> and environment variables.
    /// </summary>
    public static T? GetSetting<T>(this IServiceCollection services, string key)
    {
        var configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
        return configuration.GetSetting<T>(key);
    }

    /// <summary>
    ///  Get a setting by enum from the configuration.
    ///  The setting is loaded from <strong>appsettings.json</strong>, <strong>appsettings.{env}.json</strong> and environment variables.
    ///  The enum value is converted to a string using the Display attribute.
    ///  The setting key is the display name of the enum value.
    ///  </summary>
    public static T? GetSetting<T>(this IServiceCollection services, System.Enum setting)
    {
        var configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
        return configuration.GetSetting<T>(setting);
    }

    /// <summary>
    /// Get a setting by name from the configuration.
    /// The setting is loaded from <strong>appsettings.json</strong>, <strong>appsettings.{env}.json</strong> and environment variables.
    /// If the setting is not found, an exception is thrown.
    /// </summary>
    public static T? GetSettingOrThrow<T>(this IServiceCollection services, string key) 
        => services.GetSetting<T>(key) ?? throw new Exception($"Setting {key} not found");

    /// <summary>
    /// Get a setting by enum from the configuration.
    /// The setting is loaded from <strong>appsettings.json</strong>, <strong>appsettings.{env}.json</strong> and environment variables.
    /// The enum value is converted to a string using the Display attribute.
    /// The setting key is the display name of the enum value.
    /// If the setting is not found, an exception is thrown.
    /// </summary>
    public static T? GetSettingOrThrow<T>(this IServiceCollection services, System.Enum setting) 
        => services.GetSetting<T>(setting) ?? throw new Exception($"Setting {setting.GetDisplayName()} not found");
}
