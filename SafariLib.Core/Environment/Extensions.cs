using Microsoft.AspNetCore.Hosting;

namespace SafariLib.Core.Environment;

public static class Extensions
{
    public static bool IsTest(this IWebHostEnvironment env) => env.EnvironmentName == "Test";
}