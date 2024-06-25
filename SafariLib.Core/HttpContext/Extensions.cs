using Microsoft.AspNetCore.Http;

namespace SafariLib.Core.HttpContext;

public static class Extensions
{
    public static string? GetRemoteIpAddress(this Microsoft.AspNetCore.Http.HttpContext context) =>
        HttpContextUtils.GetRemoteIpAddressFromRequest(context.Request);

    public static string? GetRemoteIpAddress(this HttpRequest request) =>
        HttpContextUtils.GetRemoteIpAddressFromRequest(request);

    public static string? GetUserAgent(this Microsoft.AspNetCore.Http.HttpContext context) =>
        HttpContextUtils.GetUserAgentFromRequest(context.Request);

    public static string? GetUserAgent(this HttpRequest request) =>
        HttpContextUtils.GetUserAgentFromRequest(request);
}