using System.Linq.Expressions;
using SafariLib.Core.Validation;

namespace SafariLib.Repositories.RepositoryService;

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
    Result<T?> GetFirstOrDefault(Expression<Func<T, bool>> expression);
    Task<Result<T?>> GetFirstOrDefaultAsync(Expression<Func<T, bool>> expression);
    public Result<T?> GetByPrimaryKey(params object?[]? id);
    public Task<Result<T?>> GetByPrimaryKeyAsync(params object?[]? id);
}