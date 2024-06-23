using Microsoft.AspNetCore.Mvc.Testing;
using Test.WebApi;
using Xunit;

namespace Test.Integration.IntegrationTest;

public abstract class IntegrationTest : IClassFixture<WebApiFactory<Program>>
{
    protected readonly HttpClient Client;
    protected readonly IConfiguration Configuration;
    protected readonly WebApplicationFactory<Program> Factory;

    protected IntegrationTest(WebApiFactory<Program> fixture)
    {
        Factory = fixture;
        Client = Factory.CreateClient();
        Configuration = new ConfigurationBuilder().AddJsonFile("appsettings.Test.json").Build();
    }
}