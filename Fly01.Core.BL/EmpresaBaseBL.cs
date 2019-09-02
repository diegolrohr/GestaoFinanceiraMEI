using Fly01.Core.Entities.Domains;
using Fly01.Core.Helpers;
using Fly01.Core.Notifications;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Fly01.Core.BL
{
    public class EmpresaBaseBL<TEntity> : DomainBaseBL<TEntity> where TEntity : EmpresaBase
    {
        private static readonly string[] _exceptions = new[] { "DataInclusao", "UsuarioInclusao" };
        private Expression<Func<TEntity, bool>> PredicateEmpresa { get; set; }
        private Guid _empresaId;
        public Guid EmpresaId
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_empresaId.ToString()))
                    throw new BusinessException("ERRO!EmpresaId não informado.");

                return _empresaId;
            }
            set
            {
                _empresaId = value;
            }
        }
        private string _appUser;

        public string AppUser
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_appUser))
                    throw new BusinessException("ERRO! AppUser não informado.");

                return _appUser;
            }
            set
            {
                _appUser = value;
            }
        }

        public bool MustConsumeMessageServiceBus { get; set; }

        public EmpresaBaseBL(AppDataContextBase context) : base(context)
        {
            PredicateEmpresa = x => x.EmpresaId == context.EmpresaId && x.Ativo;
            AppUser = context.AppUser;
            EmpresaId = context.EmpresaId;
        }

        public override IQueryable<TEntity> All => base.All.Where(PredicateEmpresa);

        public override IQueryable<TEntity> AllWithInactive => base.AllWithInactive.Where(x => x.EmpresaId == EmpresaId);

        public virtual IQueryable<TEntity> AllWithoutPlatform => base.repository.All;

        public override IQueryable<TEntity> AllWithInactiveIncluding(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return base.AllIncluding(includeProperties).Where(x => x.EmpresaId == EmpresaId);
        }

        public virtual void ValidaModel(TEntity entity)
        {
            if (!entity.IsValid())
                throw new BusinessException(entity.Notification.Get());
        }

        public virtual new void Insert(TEntity entity)
        {
            entity.EmpresaId = EmpresaId;
            entity.DataInclusao = DateTime.Now;
            entity.DataAlteracao = null;
            entity.DataExclusao = null;
            entity.UsuarioInclusao = AppUser;
            entity.UsuarioAlteracao = null;
            entity.UsuarioExclusao = null;
            if (!entity.Ativo)
            {
                entity.UsuarioExclusao = AppUser;
                entity.DataExclusao = DateTime.Now;
            }


            ValidaModel(entity);

            if (entity.Id == default(Guid) || entity.Id == null)
                entity.Id = Guid.NewGuid();

            repository.Insert(entity);
        }

        public virtual new void Update(TEntity entity)
        {
            entity.EmpresaId = EmpresaId;
            entity.DataAlteracao = DateTime.Now;
            entity.DataExclusao = null;
            entity.UsuarioAlteracao = AppUser;
            entity.UsuarioExclusao = null;
            if (!entity.Ativo)
            {
                entity.UsuarioExclusao = AppUser;
                entity.DataExclusao = DateTime.Now;
            }

            ValidaModel(entity);
        }

        public override void AttachForUpdate(TEntity entity)
        {
            var objectDB = Find(entity.Id);

            if (objectDB == null) return;

            entity.CopyProperties<TEntity>(objectDB, _exceptions);
        }

        public virtual new void Delete(TEntity entityToDelete)
        {
            entityToDelete.Ativo = false;
            entityToDelete.DataExclusao = DateTime.Now;
            entityToDelete.UsuarioExclusao = AppUser;

            repository.Delete(entityToDelete);
        }

        public virtual void Delete(Guid id) => Delete(Find(id));

        public override IQueryable<TEntity> AllIncluding(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return base.AllIncluding(includeProperties).Where(PredicateEmpresa);
        }

        public virtual void AfterSave(TEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}