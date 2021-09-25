using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DatingApp.Core.Models;
using Microsoft.EntityFrameworkCore.Query;

namespace DatingApp.Core.Interfaces.Database.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        void Add(TEntity entity);

        void AddRange(IEnumerable<TEntity> entities);

        void Remove(TEntity entity);

        void RemoveRange(IEnumerable<TEntity> entities);

        Task<TEntity> FindByIdAsync(object id);

        Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> filter);

        /// <summary>
        /// Generic method to filter and return paginated results.
        /// </summary>
        /// <param name="filter">Filters and conditions to apply.</param>
        /// <param name="page">Current page number.</param>
        /// <param name="limit">Pagination size/limit to return.</param>
        /// <param name="include">Related objects to include and return.</param>
        /// <param name="ordination">Entities to order by.</param>
        /// <returns>Paginated entities.</returns>
        Task<Paginated<TEntity>> PaginatedFilterAsync(
            Expression<Func<TEntity, bool>> filter,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> ordination = null,
            int page = 1,
            int limit = 10
        );

        Task<Paginated<TEntity>> PaginatedFilterAsync(IQueryable<TEntity> query, int page = 1, int limit = 10);
    }
}
