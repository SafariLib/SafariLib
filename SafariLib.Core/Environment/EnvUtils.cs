using Microsoft.AspNetCore.Hosting;

namespace SafariLib.Core.Environment;

public static class EnvUtils
{
    /// <summary>
    ///    Get the current environment name.
    ///    If the environment variable ASPNETCORE_ENVIRONMENT is not set.
    ///  </summary>
    public static readonly string Env =
        System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") 
            ?? throw new System.Exception("Environment variable ASPNETCORE_ENVIRONMENT is not set.");

    /// <summary>
    ///    Check if the current environment is "Test".
    /// </summary>
    public static bool IsTest(this IWebHostEnvironment env) => env.EnvironmentName == "Test";
}