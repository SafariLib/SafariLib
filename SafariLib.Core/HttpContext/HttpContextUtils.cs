using Microsoft.AspNetCore.Http;

namespace SafariLib.Core.HttpContext;

public static class HttpContextUtils
{
    public static string? GetRemoteIpAddressFromRequest(HttpRequest request)
    {
        request.Headers.TryGetValue("X-Forwarded-For", out var forwardedFor);
        var ipAddress = forwardedFor.FirstOrDefault() ??
                        request.HttpContext.Connection.RemoteIpAddress?.ToString();
        return ipAddress;
    }

    public static string? GetUserAgentFromRequest(HttpRequest request) =>
        request.Headers.UserAgent.ToString();
}