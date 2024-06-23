namespace SafariLib.Core.Environment;

public static class EnvUtils
{
    public static readonly string Env =
        System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
}