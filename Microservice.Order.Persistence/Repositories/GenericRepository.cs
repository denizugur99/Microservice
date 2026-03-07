using Microservice.Order.Application.Contracts.Repositories;
using Microservice.Order.Domain.Entitites;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Microservice.Order.Persistence.Repositories
{

    public class GenericRepository<TId, TEntity>(AppDbContext context) : IGenericRepository<TId, TEntity>
     where TId : struct
     where TEntity : BaseEntity<TId>
   
    {
        protected AppDbContext Context=context;
        private readonly DbSet<TEntity> _dbSet = context.Set<TEntity>();

        public async ValueTask AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public Task<bool> AnyAsync(TId id)
        {
            return _dbSet.AnyAsync(x => x.Id.Equals(id));
        }

        public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
           return _dbSet.AnyAsync(predicate);
        }

        public void Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        public Task<List<TEntity>> GetAllAsync()
        {
            return _dbSet.ToListAsync();
        }

        public Task<List<TEntity>> GetAllPagedAsync(int pageNumber, int pageSize)
        {
            return _dbSet.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public ValueTask<TEntity?> GetByIdAsync(TId id)
        {
            return _dbSet.FindAsync(id);
        }

        public void Update(TEntity entity)
        {
           _dbSet.Update(entity);
        }

        public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet.Where(predicate);
        }
    }
}
