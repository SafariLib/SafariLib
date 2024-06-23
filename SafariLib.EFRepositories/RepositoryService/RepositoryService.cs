using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SafariLib.Core.Validation;
using SafariLib.EFRepositories.Repository;

namespace SafariLib.EFRepositories.RepositoryService;

public class RepositoryService<TC, T>(IRepository<TC, T> repository) : IRepositoryService<T>
    where TC : DbContext
    where T : class
{
    public void Create(T entity) => repository.Create(entity);

    public void Update(T entity) => repository.Update(entity);

    public void Delete(T entity) => repository.Delete(entity);

    public Result<List<T>> Get(Expression<Func<T, bool>> expression) =>
        new([.. repository.Get(expression)]);

    public async Task<Result<List<T>>> GetAsync(Expression<Func<T, bool>> expression) =>
        new(await repository.Get(expression).ToListAsync());

    public Result<T?> GetById(Guid id) => new(repository.GetById(id));

    public async Task<Result<T?>> GetByIdAsync(Guid id) =>
        new(await repository.GetByIdAsync(id));

    public void Save() => repository.Save();

    public async Task SaveAsync() => await repository.SaveAsync();
}