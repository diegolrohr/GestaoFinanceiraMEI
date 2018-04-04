using System;
using System.Linq;
using Fly01.Core.Base;
using System.Data.Entity;
using Fly01.Core.Api.Domain;
using System.Linq.Expressions;
using Fly01.Core.ValueObjects;

namespace Fly01.Core.Api.BL
{
    public class DomainBaseBL<TEntity> where TEntity : DomainBase
    {
        protected GenericRepository<TEntity> repository;

        public DomainBaseBL(DbContext context)
        {
            repository = new GenericRepository<TEntity>(context);
        }

        public virtual IQueryable<TEntity> All
        {
            get
            {
                return repository.All;
            }
        }

        public virtual IQueryable<TEntity> AllIncluding(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return repository.AllIncluding(includeProperties);
        }
        
        public virtual TEntity Find(object id)
        {
            return repository.Find(id);
        }

        public void Delete(int id)
        {
            throw new BusinessException("Exclusão não permitida.");
        }

        public void Delete(TEntity entityToDelete)
        {
            throw new BusinessException("Exclusão não permitida.");
        }

        public void Insert(TEntity entity)
        {
            throw new BusinessException("Inclusão não permitida.");
        }

        public void Update(TEntity entity)
        {
            throw new BusinessException("Atualização não permitida.");
        }

        public virtual void AttachForUpdate(TEntity entity)
        {
            if (entity.Id != default(Guid) && entity.Id != null)
                repository.AttachForUpdate(entity);
        }

        public virtual bool Exists(object primaryKey)
        {
            return repository.Exists(primaryKey);
        }

        public virtual void DetachEntity(TEntity entityToDetach)
        {
            repository.DetachEntity(entityToDetach);
        }
    }
}
