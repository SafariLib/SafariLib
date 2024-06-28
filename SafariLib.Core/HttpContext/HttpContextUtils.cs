using Microsoft.AspNetCore.Http;

namespace SafariLib.Core.HttpContext;

public static class HttpContextUtils
{
    /// <summary>
    ///     Get the remote IP address from the request.
    /// </summary>
    public static string? GetRemoteIpAddressFromRequest(HttpRequest request)
    {
        request.Headers.TryGetValue("X-Forwarded-For", out var forwardedFor);
        var ipAddress = forwardedFor.FirstOrDefault() ??
                        request.HttpContext.Connection.RemoteIpAddress?.ToString();
        return ipAddress;
    }

    /// <summary>
    ///     Get the remote IP address from the request.
    /// </summary>
    public static string? GetRemoteIpAddress(this Microsoft.AspNetCore.Http.HttpContext context) =>
        HttpContextUtils.GetRemoteIpAddressFromRequest(context.Request);

    /// <summary>
    ///     Get the remote IP address from the request.
    /// </summary>
    public static string? GetRemoteIpAddress(this HttpRequest request) =>
        HttpContextUtils.GetRemoteIpAddressFromRequest(request);

    /// <summary>
    ///     Get the user agent from the request.
    /// </summary>
    public static string? GetUserAgentFromRequest(HttpRequest request) =>
        request.Headers.UserAgent.ToString();

    /// <summary>
    ///     Get the user agent from the request.
    /// </summary>
    public static string? GetUserAgent(this Microsoft.AspNetCore.Http.HttpContext context) =>
        HttpContextUtils.GetUserAgentFromRequest(context.Request);
    
    /// <summary>
    ///     Get the user agent from the request.
    /// </summary>
    public static string? GetUserAgent(this HttpRequest request) =>
        HttpContextUtils.GetUserAgentFromRequest(request);
}