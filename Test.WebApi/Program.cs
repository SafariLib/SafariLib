using SafariLib.Repositories.Repository;
using SafariLib.Repositories.RepositoryService;
using Test.Utils.Data;
using Test.Utils.Data.Models;

namespace Test.WebApi;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllers();
        builder.Services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
        builder.Services.AddScoped<IRepositoryService<User>, RepositoryService<TestContext, User>>();
        builder.Services.AddScoped<IRepositoryService<UserWithGuid>, RepositoryService<TestContext, UserWithGuid>>();
        builder.Build().Run();
    }
}