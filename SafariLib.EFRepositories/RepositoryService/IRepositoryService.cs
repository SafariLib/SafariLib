using System.Linq.Expressions;
using SafariLib.Core.Validation;

namespace SafariLib.EFRepositories.RepositoryService;

public interface IRepositoryService<T>
    where T : class
{
    public void Create(T entity);
    public void Delete(T entity);
    public void Update(T entity);
    public void Save();
    public Task SaveAsync();
    public Result<List<T>> Get(Expression<Func<T, bool>> expression);
    public Task<Result<List<T>>> GetAsync(Expression<Func<T, bool>> expression);
    Result<T?> GetById(Guid id);
    Task<Result<T?>> GetByIdAsync(Guid id);
}