using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DatingApp.Core.Models;

namespace DatingApp.Core.Interfaces.Database.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        void Add(TEntity entity);

        void AddRange(IEnumerable<TEntity> entities);

        void Remove(TEntity entity);

        void RemoveRange(IEnumerable<TEntity> entities);

        Task<TEntity> FindByIdAsync(object id);

        Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, bool>> include = null);

        Task<IEnumerable<TEntity>> FilterAsync(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, bool>> include = null);

        Task<PagedResult<TEntity>> FilterAsync(
            Expression<Func<TEntity, bool>> filter,
            int page = 1,
            int pageSize = 10,
            Expression<Func<TEntity, bool>> include = null,
            Expression<Func<TEntity, bool>> orderBy = null,
            Expression<Func<TEntity, bool>> orderByDescending = null
        );

        Task<PagedResult<TEntity>> FilterAsync(IQueryable<TEntity> query, int page = 1, int pageSize = 10);
    }
}
