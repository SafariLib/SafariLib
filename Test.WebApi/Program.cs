using SafariLib.EFRepositories;

namespace Test.WebApi;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllers();
        builder.Services.AddEFRepositories();
        builder.Build().Run();
    }
}