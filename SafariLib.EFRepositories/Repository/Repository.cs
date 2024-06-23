using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SafariLib.EFRepositories.Models;

namespace SafariLib.EFRepositories.Repository;

public class Repository<TC, T>(TC context) : IRepository<TC, T>
    where TC : DbContext
    where T : class
{
    public void Create(T entity) => context.Set<T>().Add(entity);

    public async Task CreateAsync(T entity) => await context.Set<T>().AddAsync(entity);

    public void Update(T entity) => context.Set<T>().Update(entity);

    public void Delete(T entity) => context.Set<T>().Remove(entity);

    public IQueryable<T> Get(Expression<Func<T, bool>> expression) =>
        context.Set<T>().Where(expression);

    public T? GetById(Guid id) => context.Set<T>().Find(id);

    public async Task<T?> GetByIdAsync(Guid id) => await context.Set<T>().FindAsync(id);

    public async Task SaveAsync()
    {
        AddTimestamps();
        await context.SaveChangesAsync();
    }

    public void Save()
    {
        AddTimestamps();
        context.SaveChanges();
    }

    private void AddTimestamps()
    {
        var now = DateTime.UtcNow;
        var entities = context
            .ChangeTracker.Entries()
            .Where(x =>
                x is { Entity: Entity, State: EntityState.Added or EntityState.Modified }
            );

        foreach (var entity in entities)
        {
            var property = entity.State is EntityState.Added ? "CreatedAt" : "UpdatedAt";
            entity.Property(property).CurrentValue = now;
        }
    }
}