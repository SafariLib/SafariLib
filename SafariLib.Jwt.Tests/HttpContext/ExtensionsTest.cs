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
    [Fact]
    public void GetBearerToken_Request_ReturnsToken()
    {
        // Arrange
        var mockRequest = new Mock<HttpRequest>();
        mockRequest
            .Setup(r => r.Headers)
            .Returns(
                new HeaderDictionary { { "Authorization", new StringValues("Bearer test_token") } }
            );

        // Act
        var result = mockRequest.Object.GetBearerToken();

        // Assert
        Assert.Equal("test_token", result);
    }

    [Fact]
    public void GetBearerToken_Context_ReturnsToken()
    {
        // Arrange
        var mockContext = new Mock<Microsoft.AspNetCore.Http.HttpContext>();
        var mockRequest = new Mock<HttpRequest>();
        mockRequest
            .Setup(r => r.Headers)
            .Returns(
                new HeaderDictionary { { "Authorization", new StringValues("Bearer test_token") } }
            );
        mockContext.Setup(c => c.Request).Returns(mockRequest.Object);

        // Act
        var result = mockContext.Object.GetBearerToken();

        // Assert
        Assert.Equal("test_token", result);
    }

    [Fact]
    public void GetJwtToken_Context_ReturnsToken()
    {
        // Arrange
        var mockContext = new Mock<Microsoft.AspNetCore.Http.HttpContext>();
        var mockRequest = new Mock<HttpRequest>();
        var mockServices = new Mock<IServiceProvider>();
        var mockJwtConfig = new Mock<IJwtConfigService>();
        mockJwtConfig.Setup(c => c.CookieName).Returns("test_cookie");
        mockServices.Setup(s => s.GetService(typeof(IJwtConfigService))).Returns(mockJwtConfig.Object);
        mockRequest.Setup(r => r.HttpContext.RequestServices).Returns(mockServices.Object);
        mockContext.Setup(c => c.Request).Returns(mockRequest.Object);
        mockContext
            .Setup(c => c.Items)
            .Returns(new Dictionary<object, object> { { "Token", new JwtToken<object>() } }!);

        // Act
        var result = mockContext.Object.GetJwtToken<object>();

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void GetJwtToken_Context_ReturnsTokenWithError()
    {
        // Arrange
        var mockContext = new Mock<Microsoft.AspNetCore.Http.HttpContext>();
        var mockRequest = new Mock<HttpRequest>();
        var mockServices = new Mock<IServiceProvider>();
        var mockJwtConfig = new Mock<IJwtConfigService>();
        mockJwtConfig.Setup(c => c.CookieName).Returns("test_cookie");
        mockServices.Setup(s => s.GetService(typeof(IJwtConfigService))).Returns(mockJwtConfig.Object);
        mockRequest.Setup(r => r.HttpContext.RequestServices).Returns(mockServices.Object);
        mockContext.Setup(c => c.Request).Returns(mockRequest.Object);
        mockContext
            .Setup(c => c.Items)
            .Returns(
                new Dictionary<object, object> { { "WrongSpelledToken", new JwtToken<object>() } }!
            );

        // Act
        var result = mockContext.Object.GetJwtToken<object>();

        // Assert
        Assert.NotNull(result);
        Assert.True(result.HasError);
        Assert.Collection(result.Errors, e => Assert.IsType<ResultMessage>(e));
    }

    [Fact]
    public void GetJwtToken_Request_ReturnsToken()
    {
        // Arrange
        var mockRequest = new Mock<HttpRequest>();
        var mockServices = new Mock<IServiceProvider>();
        var mockJwtConfig = new Mock<IJwtConfigService>();
        mockJwtConfig.Setup(c => c.CookieName).Returns("test_cookie");
        mockServices.Setup(s => s.GetService(typeof(IJwtConfigService))).Returns(mockJwtConfig.Object);
        mockRequest.Setup(r => r.HttpContext.RequestServices).Returns(mockServices.Object);
        mockRequest
            .Setup(r => r.HttpContext.Items)
            .Returns(new Dictionary<object, object> { { "Token", new JwtToken<object>() } }!);

        // Act
        var result = mockRequest.Object.GetJwtToken<object>();

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void GetCookieToken_ReturnsToken()
    {
        // Arrange
        var mockRequest = new Mock<HttpRequest>();
        var mockServices = new Mock<IServiceProvider>();
        var mockJwtConfig = new Mock<IJwtConfigService>();
        mockJwtConfig.Setup(c => c.CookieName).Returns("RefreshToken");
        mockServices.Setup(s => s.GetService(typeof(IJwtConfigService))).Returns(mockJwtConfig.Object);
        mockRequest.Setup(r => r.HttpContext.RequestServices).Returns(mockServices.Object);
        mockRequest
            .Setup(r => r.Cookies)
            .Returns(Mock.Of<IRequestCookieCollection>(c => c["RefreshToken"] == "test_token"));

        // Act
        var result = mockRequest.Object.GetCookieToken();

        // Assert
        Assert.Equal("test_token", result);
    }

    [Fact]
    public void SetCookieToken_SetsCookie()
    {
        // Arrange
        var mockResponse = new Mock<HttpResponse>();
        var mockCookies = new Mock<IResponseCookies>();
        var mockServices = new Mock<IServiceProvider>();
        var mockJwtConfig = new Mock<IJwtConfigService>();
        mockJwtConfig.Setup(c => c.CookieName).Returns("test_cookie");
        mockJwtConfig.Setup(c => c.BearerTokenExpiration).Returns(1000);
        mockServices.Setup(s => s.GetService(typeof(IJwtConfigService))).Returns(mockJwtConfig.Object);
        mockResponse.Setup(r => r.HttpContext.RequestServices).Returns(mockServices.Object);
        mockResponse.Setup(r => r.Cookies).Returns(mockCookies.Object);

        // Act
        mockResponse.Object.SetCookieToken("test_token");

        // Assert
        mockCookies.Verify(
            c => c.Append("test_cookie", "test_token", It.IsAny<CookieOptions>()),
            Times.Once
        );
    }

    [Fact]
    public void RemoveCookieToken_RemovesCookie()
    {
        // Arrange
        var mockResponse = new Mock<HttpResponse>();
        var mockCookies = new Mock<IResponseCookies>();
        var mockServices = new Mock<IServiceProvider>();
        var mockJwtConfig = new Mock<IJwtConfigService>();
        mockJwtConfig.Setup(c => c.CookieName).Returns("test_cookie");
        mockServices.Setup(s => s.GetService(typeof(IJwtConfigService))).Returns(mockJwtConfig.Object);
        mockResponse.Setup(r => r.HttpContext.RequestServices).Returns(mockServices.Object);
        mockResponse.Setup(r => r.Cookies).Returns(mockCookies.Object);

        // Act
        mockResponse.Object.RemoveCookieToken();

        // Assert
        mockCookies.Verify(c => c.Delete("test_cookie"), Times.Once);
    }
}