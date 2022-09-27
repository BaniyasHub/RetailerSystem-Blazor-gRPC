using Microsoft.EntityFrameworkCore;
using Retailer.Data.Models;
using Retailer.DataAccess.Context;
using Retailer.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Retailer.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : BaseModel, new()
    {
        private readonly RetailerDbContext context;
        internal DbSet<T> dbSet;

        public Repository(RetailerDbContext context)
        {
            this.context = context;
            this.dbSet = context.Set<T>();
        }

        public async Task<T> FindOne(Expression<Func<T, bool>> predicate, bool isTracking = true, params string[] includeProperties)
        {
            IQueryable<T> query = dbSet;

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (includeProperties != null && includeProperties.Count() > 0)
            {
                query = query.Include(includeProperties?.First());

                for (int i = 1; i < includeProperties.Length; i++)
                {
                    query = query.Include(includeProperties[i]);
                }
            }

            if (isTracking == false)
            {
                query = query.AsNoTracking();
            }

            return await query.SingleOrDefaultAsync();
        }

        public async Task<IQueryable<T>> FindMany(Expression<Func<T, bool>> predicate, bool isTracking = true, params string[] includePaths)
        {
            IQueryable<T> query = dbSet;

            if (isTracking != true)
            {
                query = query.AsNoTracking();
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (includePaths != null && includePaths.Count() > 0)
            {
                query = query.Include(includePaths?.First());

                for (int i = 1; i < includePaths.Length; i++)
                {
                    query = query.Include(includePaths[i]);
                }
            }


            return query;
        }

        public async Task<IQueryable<T>> FindMany2(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
          bool isTracking = true, params string[] includePaths)
        {
            IQueryable<T> query = dbSet;

            if (isTracking != true)
            {
                query = query.AsNoTracking();
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (includePaths != null)
            {
                for (int i = 1; i < includePaths.Length; i++)
                {
                    query = query.Include(includePaths[i]);
                }
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return query;
        }

        public async Task<int> Count(Expression<Func<T, bool>> predicate)
        {
            return await dbSet.Where(predicate).CountAsync();
        }

        public async Task Add(T entity)
        {
           await dbSet.AddAsync(entity);
        }

        public async Task Delete(T entity)
        {
            dbSet.Remove(entity);
        }

        public async Task DeleteRange(IEnumerable<T> entities)
        {
            dbSet.RemoveRange(entities);
        }

        public async Task<bool> SaveChanges()
        {
            return await context.SaveChangesAsync() >= 0;
        }


    }
}
