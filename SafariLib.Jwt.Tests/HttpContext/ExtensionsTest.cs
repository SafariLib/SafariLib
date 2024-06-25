using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Moq;
using SafariLib.Core.Validation;
using SafariLib.Jwt.HttpContext;
using SafariLib.Jwt.Models;
using SafariLib.Jwt.Services;

namespace SafariLib.Jwt.Tests.HttpContext;

public class ExtensionsTest
{
    private const string DefaultBearerToken = "test_token";
    private const string DefaultCookieName = "test_cookie";
    private const string DefaultBearerName = "Token";
    private const string RefreshTokenCookieName = "RefreshToken";

    private readonly Mock<Microsoft.AspNetCore.Http.HttpContext> mockContext = new();
    private readonly Mock<HttpRequest> mockRequest = new();
    private readonly Mock<HttpResponse> mockResponse = new();
    private readonly Mock<IResponseCookies> mockCookies = new();
    private readonly Mock<IServiceProvider> mockServices = new();
    private readonly Mock<IJwtService> mockJwtService = new();

    public ExtensionsTest()
    {
        SetupMocks();
    }

    private void SetupMocks(HeaderDictionary? headers = null, string? cookieName = null, string? bearerName = null)
    {
        mockServices.Setup(provider => provider.GetService(typeof(IJwtService)))
                .Returns(mockJwtService.Object);
        mockResponse.Setup(r => r.HttpContext.RequestServices).Returns(mockServices.Object);
        mockResponse.Setup(r => r.Cookies).Returns(mockCookies.Object);
        mockRequest.Setup(r => r.HttpContext.RequestServices).Returns(mockServices.Object);
        mockRequest.Setup(r => r.Headers).Returns(headers ?? new HeaderDictionary { { "Authorization", new StringValues($"Bearer {DefaultBearerToken}") } });
        mockRequest.Setup(r => r.HttpContext.Items).Returns(new Dictionary<object, object> { { bearerName ?? DefaultBearerName, new JwtToken<object>() } });
        mockRequest.Setup(r => r.Cookies).Returns(Mock.Of<IRequestCookieCollection>(c => c[cookieName ?? RefreshTokenCookieName] == DefaultBearerToken));
        mockContext.Setup(c => c.Items).Returns(new Dictionary<object, object> { { bearerName ?? DefaultBearerName, new JwtToken<object>() } });
        mockContext.Setup(c => c.Request).Returns(mockRequest.Object);
        mockJwtService.Setup(c => c.GetBearerTokenExpiration()).Returns(1000);
        mockJwtService.Setup(c => c.GetCookieName()).Returns(cookieName ?? DefaultCookieName);
    }

    [Fact]
    public void GetBearerToken_Request_ReturnsToken()
    {
        var result = mockRequest.Object.GetBearerToken();
        Assert.Equal(DefaultBearerToken, result);
    }

    [Fact]
    public void GetBearerToken_Context_ReturnsToken()
    {
        var result = mockContext.Object.GetBearerToken();
        Assert.Equal(DefaultBearerToken, result);
    }

    [Fact]
    public void GetJwtToken_Context_ReturnsToken()
    {
        var result = mockContext.Object.GetJwtToken<object>();
        Assert.NotNull(result);
    }

    [Fact]
    public void GetJwtToken_Context_ReturnsTokenWithError()
    {
        SetupMocks(bearerName: "WrongSpelledToken");
        var result = mockContext.Object.GetJwtToken<object>();
        Assert.NotNull(result);
        Assert.True(result.HasError);
        Assert.Collection(result.Errors, e => Assert.IsType<ResultMessage>(e));
    }

    [Fact]
    public void GetJwtToken_Request_ReturnsToken()
    {
        var result = mockRequest.Object.GetJwtToken<object>();
        Assert.NotNull(result);
    }

    [Fact]
    public void GetCookieToken_ReturnsToken()
    {
        SetupMocks(cookieName: RefreshTokenCookieName);
        var result = mockRequest.Object.GetCookieToken();
        Assert.Equal(DefaultBearerToken, result);
    }

    [Fact]
    public void SetCookieToken_SetsCookie()
    {
        mockResponse.Object.SetCookieToken(DefaultBearerToken);
        mockCookies.Verify(c => c.Append(DefaultCookieName, DefaultBearerToken, It.IsAny<CookieOptions>()), Times.Once);
    }

    [Fact]
    public void RemoveCookieToken_RemovesCookie()
    {
        mockResponse.Object.RemoveCookieToken();
        mockCookies.Verify(c => c.Delete(DefaultCookieName), Times.Once);
    }
}