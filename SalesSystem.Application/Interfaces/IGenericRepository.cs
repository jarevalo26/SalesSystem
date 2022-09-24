using System.Linq.Expressions;

namespace SalesSystem.Application.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<TEntity> Get(Expression<Func<TEntity, bool>> filter);
        Task<TEntity> Create(TEntity entity);
        Task<bool> Update(TEntity entity);
        Task<bool> Delete(TEntity entity);
        Task<IQueryable<TEntity>> GetAll(Expression<Func<TEntity, bool>>? filter = null);
    }
}
