using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using SafariLib.Jwt.Models;
using SafariLib.Jwt.Services;

namespace SafariLib.Jwt.HttpContext;

public static class Extensions
{
    public static string? GetBearerToken(this HttpRequest request) =>
        request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();

    public static string? GetBearerToken(this Microsoft.AspNetCore.Http.HttpContext context) =>
        context.Request.GetBearerToken();

    public static string? GetCookieToken(this HttpRequest request)
    {
        var jwtConfig = request.HttpContext.RequestServices.GetRequiredService<IJwtConfigService>();
        return request.Cookies[jwtConfig.CookieName];
    }

    public static JwtToken<T> GetJwtToken<T>(this HttpRequest request) =>
        request.HttpContext.GetJwtToken<T>();

    public static JwtToken<T> GetJwtToken<T>(this Microsoft.AspNetCore.Http.HttpContext context)
    {
        var result = new JwtToken<T>();
        try
        {
            return context.Items["Token"] is not string item
                ? result
                : JsonSerializer.Deserialize<JwtToken<T>>(item)!;
        }
        catch (Exception e)
        {
            return new JwtToken<T>().AddError(e);
        }
    }

    public static void SetCookieToken(this HttpResponse response, string token)
    {
        var jwtConfig = response.HttpContext.RequestServices.GetRequiredService<IJwtConfigService>();
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTime.UtcNow.AddMilliseconds(jwtConfig.BearerTokenExpiration)
        };
        response.Cookies.Append(jwtConfig.CookieName, token, cookieOptions);
    }

    public static void RemoveCookieToken(this HttpResponse response)
    {
        var jwtConfig = response.HttpContext.RequestServices.GetRequiredService<IJwtConfigService>();
        response.Cookies.Delete(jwtConfig.CookieName);
    }
}