using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Notifications;
using System;
using System.Data.Entity;
using System.Linq;

namespace Fly01.OrdemServico.BL.Base
{
    public abstract class OrdemServicoItemBLBase<TEntity> : PlataformaBaseBL<TEntity> where TEntity : OrdemServicoItemBase
    {
        private readonly GenericRepository<Core.Entities.Domains.Commons.OrdemServico> _repositoryOS;

        public OrdemServicoItemBLBase(AppDataContextBase context) : base(context)
        {
            _repositoryOS = new GenericRepository<Core.Entities.Domains.Commons.OrdemServico>(context);
        }

        public override void Delete(TEntity entityToDelete)
        {
            var id = entityToDelete.Id;
            var os = GetOrdemServico(entityToDelete);

            if (os != null) ValidarOSDelete(entityToDelete, os, id);

            if (!entityToDelete.IsValid())
                throw new BusinessException(entityToDelete.Notification.Get());
            base.Delete(entityToDelete);
        }

        public override void Update(TEntity entity)
        {
            var os = GetOrdemServico(entity);

            if (os != null) ValidarOSUpdate(entity, os, entity.Id);

            base.Update(entity);
        }

        public override void Insert(TEntity entity)
        {
            var os = GetOrdemServico(entity);

            if (os != null) ValidarOSInsert(entity, os, entity.Id);

            base.Insert(entity);
        }

        protected virtual void ValidarOSInsert(TEntity entity, Core.Entities.Domains.Commons.OrdemServico os, Guid id) => ValidarOS(entity, os, id);
        protected virtual void ValidarOSUpdate(TEntity entity, Core.Entities.Domains.Commons.OrdemServico os, Guid id) => ValidarOS(entity, os, id);
        protected virtual void ValidarOSDelete(TEntity entity, Core.Entities.Domains.Commons.OrdemServico os, Guid id) => ValidarOS(entity, os, id);

        protected virtual void ValidarOS(TEntity entity, Core.Entities.Domains.Commons.OrdemServico os, Guid id)
            => entity.Fail(os.Status != StatusOrdemServico.EmAberto && os.Status != StatusOrdemServico.EmAndamento && os.Status != StatusOrdemServico.EmPreenchimento,
                        new Error("Só é permitido editar ordens 'Em Preenchimento', 'Em Aberto' e 'Em Andamento'", "status"));

        private Core.Entities.Domains.Commons.OrdemServico GetOrdemServico(TEntity entity)
            => _repositoryOS.All.AsNoTracking().FirstOrDefault(e => e.Id == entity.OrdemServicoId && e.PlataformaId == PlataformaUrl);
    }
}
