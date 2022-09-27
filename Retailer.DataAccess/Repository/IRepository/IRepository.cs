using Retailer.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Retailer.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : BaseModel
    {
        Task<T> FindOne(Expression<Func<T, bool>> predicate, bool isTracking = true, params string[] includeProperties);

        Task<IQueryable<T>> FindMany(Expression<Func<T, bool>> predicate, bool isTracking = true, params string[] includePaths);

        Task<IQueryable<T>> FindMany2(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
          bool isTracking = true, params string[] includePaths);
        Task<int> Count(Expression<Func<T, bool>> predicate);

        Task Add(T entity);

        Task Delete(T entity);

        Task DeleteRange(IEnumerable<T> entities);

        Task<bool> SaveChanges();
    }
}
