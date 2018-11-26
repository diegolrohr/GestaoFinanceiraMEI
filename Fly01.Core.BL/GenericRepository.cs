using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Fly01.Core.Entities.Domains;

namespace Fly01.Core.BL
{
    public class GenericRepository<TEntity> where TEntity : DomainBase
    {
        internal DbContext context;
        internal DbSet<TEntity> dbSet;

        public GenericRepository(DbContext context)
        {
            this.context = context;
            dbSet = context.Set<TEntity>();
        }

        public IQueryable<TEntity> AllWithInactive
        {
            get
            {
                return dbSet;
            }
        }

        public IQueryable<TEntity> AllWithInactiveIncluding(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = this.dbSet;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public IQueryable<TEntity> All
        {
            get
            {
                return dbSet.Where(x => x.Ativo);
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

        public List<DbEntityEntry> GetAffectEntries()
        {
            List<DbEntityEntry> entries = new List<DbEntityEntry>();
            foreach (var entry in context.ChangeTracker.Entries().Where(p => p.State == EntityState.Added || p.State == EntityState.Modified))
                entries.Add(entry);

            return entries;
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

        public virtual void Insert(TEntity entity)
        {
            dbSet.Add(entity);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            //if (context.Entry(entityToDelete).State == EntityState.Detached)
            //{
            //    dbSet.Attach(entityToDelete);
            //}

            //!dbSet.Remove(entityToDelete);
        }

        public virtual void Delete(object id)
        {
            Delete(Find(id));
        }

        public virtual void AttachForUpdate(TEntity entityToUpdate)
        {
            dbSet.Attach(entityToUpdate);
        }
        
        public bool Exists(object primaryKey)
        {
            return Find(primaryKey) != null;
        }

        public virtual void DetachEntity(TEntity entityToDetach)
        {
            context.Entry(entityToDetach).State = EntityState.Detached;
        }
    }
}
