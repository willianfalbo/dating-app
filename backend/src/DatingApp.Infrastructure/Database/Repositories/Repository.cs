using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DatingApp.Core.Interfaces.Database.Repositories;
using DatingApp.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace DatingApp.Infrastructure.Database.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly DatabaseContext _context;

        public Repository(DatabaseContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void Add(TEntity entity) =>
             _context.Set<TEntity>().Add(entity);

        public void AddRange(IEnumerable<TEntity> entities) =>
             _context.Set<TEntity>().AddRange(entities);

        public void Remove(TEntity entity) =>
            _context.Set<TEntity>().Remove(entity);

        public void RemoveRange(IEnumerable<TEntity> entities) =>
            _context.Set<TEntity>().RemoveRange(entities);

        public async Task<TEntity> FindByIdAsync(object id) =>
            await _context.Set<TEntity>().FindAsync(id);

        public async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> filter) =>
            await _context.Set<TEntity>().FirstOrDefaultAsync(filter);

        /// <inheritdoc />
        // implementation based on https://stackoverflow.com/a/63764885/11644167.
        public async Task<Paginated<TEntity>> PaginatedFilterAsync(
            Expression<Func<TEntity, bool>> filter,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> ordination = null,
            int page = 1,
            int limit = 10
        )
        {
            var query = _context.Set<TEntity>().AsQueryable();

            if (include != null)
                query = include(query);

            if (ordination != null)
                query = ordination(query);

            if (filter != null)
                query = query.Where(filter);

            var count = await query.CountAsync();
            var items = await query.Skip((page - 1) * limit).Take(limit).ToListAsync();

            return new Paginated<TEntity>(items, count, page, limit);
        }

        public async Task<Paginated<TEntity>> PaginatedFilterAsync(IQueryable<TEntity> query, int page = 1, int limit = 10)
        {
            var count = await query.CountAsync();
            var items = await query.Skip((page - 1) * limit).Take(limit).ToListAsync();

            return new Paginated<TEntity>(items, count, page, limit);
        }
    }
}
