using Microsoft.Extensions.DependencyInjection;
using SafariLib.EFRepositories.Repository;

namespace SafariLib.EFRepositories;

public static class Injector
{
    /// <summary>
    ///     Adds the EF repositories library. This method should be called in the ConfigureServices method of the Startup
    ///     class.
    /// </summary>
    public static IServiceCollection AddEFRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
        return services;
    }
}