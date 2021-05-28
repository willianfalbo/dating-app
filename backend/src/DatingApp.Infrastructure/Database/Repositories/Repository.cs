using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DatingApp.Core.Interfaces.Database.Repositories;
using DatingApp.Core.Models;
using Microsoft.EntityFrameworkCore;

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

        public async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, bool>> include = null) =>
            await _context.Set<TEntity>()
                .Include(include)
                .FirstOrDefaultAsync(filter);

        public async Task<IEnumerable<TEntity>> FilterAsync(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, bool>> include = null)
        {
            var query = _context.Set<TEntity>().AsQueryable();

            if (filter != null)
                query = query.Where(filter);

            if (include != null)
                query = query.Include(include);

            return await query.ToListAsync();
        }

        public async Task<PagedResult<TEntity>> FilterAsync(
            Expression<Func<TEntity, bool>> filter,
            int page = 1,
            int pageSize = 10,
            Expression<Func<TEntity, bool>> include = null,
            Expression<Func<TEntity, bool>> orderBy = null,
            Expression<Func<TEntity, bool>> orderByDescending = null
        )
        {
            var query = _context.Set<TEntity>().AsQueryable();

            if (orderBy != null)
                query = query.OrderBy(orderBy);

            if (orderByDescending != null)
                query = query.OrderByDescending(orderByDescending);

            if (filter != null)
                query = query.Where(filter);

            if (include != null)
                query = query.Include(include);

            var count = await query.CountAsync();
            var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PagedResult<TEntity>(items, count, page, pageSize);
        }

        public async Task<PagedResult<TEntity>> FilterAsync(IQueryable<TEntity> query, int page = 1, int pageSize = 10)
        {
            var count = await query.CountAsync();
            var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PagedResult<TEntity>(items, count, page, pageSize);
        }
    }
}
