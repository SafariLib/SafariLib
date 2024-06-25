using Microsoft.EntityFrameworkCore;
using SafariLib.Core.Validation;
using SafariLib.Repositories.Models;

namespace SafariLib.Repositories.RepositoryService;

public static class RepositoryServiceExtensions
{
    public static Result<T?> GetById<T>(this IRepositoryService<T> repository, Guid id)
        where T : EntityWithGuid
        => repository.GetByPrimaryKey(id);

    public static async Task<Result<T?>> GetByIdAsync<T>(this IRepositoryService<T> repository, Guid id)
        where T : EntityWithGuid
        => await repository.GetByPrimaryKeyAsync(id);

    public static Result<T?> GetById<T>(this IRepositoryService<T> repository, int id)
        where T : Entity => repository.GetByPrimaryKey(id);

    public static async Task<Result<T?>> GetByIdAsync<T>(this IRepositoryService<T> repository, int id)
        where T : Entity => await repository.GetByPrimaryKeyAsync(id);

    public static Result<T?> GetById<TC, T>(this RepositoryService<TC, T> repository, Guid id)
        where TC : DbContext
        where T : EntityWithGuid
        => repository.GetByPrimaryKey(id);

    public static async Task<Result<T?>> GetByIdAsync<TC, T>(this RepositoryService<TC, T> repository, Guid id)
        where TC : DbContext
        where T : EntityWithGuid
        => await repository.GetByPrimaryKeyAsync(id);

    public static Result<T?> GetById<TC, T>(this RepositoryService<TC, T> repository, int id)
        where TC : DbContext
        where T : Entity => repository.GetByPrimaryKey(id);

    public static async Task<Result<T?>> GetByIdAsync<TC, T>(this RepositoryService<TC, T> repository, int id)
        where TC : DbContext
        where T : Entity => await repository.GetByPrimaryKeyAsync(id);
}