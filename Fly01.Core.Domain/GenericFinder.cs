using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Fly01.Core.Domain
{
    public class GenericFinder<TEntity> where TEntity : class 
    {
        internal DbContext context;
        internal DbSet<TEntity> dbSet;

        public GenericFinder(DbContext context)
        {
            this.context = context;
            dbSet = context.Set<TEntity>();
        }

        public IQueryable<TEntity> All
        {
            get
            {
                return dbSet;
            }
        }

        public IQueryable<TEntity> AllIncluding(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = this.dbSet;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public bool Any(Expression<Func<TEntity, bool>> predicate)
        {
            return dbSet.Any(predicate);
        }

        public virtual TEntity Find(object id)
        {
            return dbSet.Find(id);
        }

        public virtual Task<TEntity> FindAsync(object id)
        {
            return dbSet.FindAsync(id);
        }

        public bool Exists(object primaryKey)
        {
            return Find(primaryKey) != null;
        }
    }
}
