using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace SafariLib.EFRepositories.Repository;

/// <summary>
///     The IRepository interface is used to define the basic CRUD operations that can be performed on a database entity.
/// </summary>
/// <typeparam name="TC">
///     The type of the DbContext that the repository will use to interact with the database.
/// </typeparam>
/// <typeparam name="T">
///     The type of the entity that the repository will interact with.
/// </typeparam>
public interface IRepository<TC, T>
    where TC : DbContext
    where T : class
{
    public void Create(T entity);
    public Task CreateAsync(T entity);
    public void Delete(T entity);
    public void Update(T entity);
    public IQueryable<T> Get(Expression<Func<T, bool>> expression);
    T? GetById(Guid id);
    Task<T?> GetByIdAsync(Guid id);
    public void Save();
    public Task SaveAsync();
}