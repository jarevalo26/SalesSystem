using Microsoft.EntityFrameworkCore;
using SalesSystem.Application.Interfaces;
using SalesSystem.Infrastructure.Contexts;
using System.Linq.Expressions;

namespace SalesSystem.Infrastructure.Implementations
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly VentasDbContext _dbContext;

        public GenericRepository(VentasDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<TEntity> Get(Expression<Func<TEntity, bool>> filter)
        {
            try
            {
                TEntity? entity = await _dbContext.Set<TEntity>().FirstOrDefaultAsync(filter);
                return entity!;
            }
            catch
            {
                throw;
            }
        }

        public async Task<TEntity> Create(TEntity entity)
        {
            try
            {
                _dbContext.Set<TEntity>().Add(entity);
                await _dbContext.SaveChangesAsync();
                return entity!;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Update(TEntity entity)
        {
            try
            {
                _dbContext.Update(entity);
                await _dbContext.SaveChangesAsync();
                return true!;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Delete(TEntity entity)
        {
            try
            {
                _dbContext.Remove(entity);
                await _dbContext.SaveChangesAsync();
                return true!;
            }
            catch
            {
                throw;
            }
        }

        public Task<IQueryable<TEntity>> GetAll(Expression<Func<TEntity, bool>>? filter = null)
        {
            IQueryable<TEntity> query = filter == null ?
                _dbContext.Set<TEntity>() :
                _dbContext.Set<TEntity>().Where(filter);
            return Task.FromResult(query);
        }
    }
}
