using System.Linq.Expressions;
using KafkaDelivery.Domain.Entities;
using KafkaDelivery.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace KafkaDelivery.Infra.Repositories;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
{
    protected readonly DeliveryDbContext Database;

    public Repository(DeliveryDbContext database)
    {
        Database = database;
    }
    
    public IQueryable<TEntity> GetAll()
    {
        return Database.Set<TEntity>().AsQueryable();
    }

    public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = Database.Set<TEntity>();

        // Aplicar includes se houver
        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        return await query.FirstOrDefaultAsync(predicate);
    }

    public async Task<TEntity> SaveAsync(TEntity entity)
    {
        Database.Set<TEntity>().Add(entity);
        
        await Database.SaveChangesAsync();
        
        return entity;
    }

    public async Task<TEntity> UpdateAsync(TEntity entity)
    {
        Database.Set<TEntity>().Update(entity);

        await Database.SaveChangesAsync();
        
        return entity;
    }

    public async Task<TEntity> DeleteAsync(TEntity entity)
    {
        Database.Set<TEntity>().Remove(entity);

        await Database.SaveChangesAsync();

        return entity;
    }
}