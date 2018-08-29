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
        private readonly OrdemServicoBL _ordemServicoBL;

        public OrdemServicoItemBLBase(AppDataContextBase context, OrdemServicoBL ordemServicoBL) : base(context)
        {
            _ordemServicoBL = ordemServicoBL;
        }

        public override void Delete(TEntity entityToDelete)
        {
            var id = entityToDelete.Id;
            var os = GetOrdemServico(id);

            if (os != null) ValidarOSDelete(os, id);

            if (!os.IsValid())
                throw new BusinessException(os.Notification.Get());
            base.Delete(entityToDelete);
        }

        public override void Update(TEntity entity)
        {
            var os = GetOrdemServico(entity.OrdemServicoId);

            if (os != null) ValidarOSUpdate(os, entity.Id);

            base.Update(entity);
        }

        public override void Insert(TEntity entity)
        {
            var os = GetOrdemServico(entity.OrdemServicoId);

            if (os != null) ValidarOSInsert(os, entity.Id);

            base.Insert(entity);
        }

        protected virtual void ValidarOSInsert(Core.Entities.Domains.Commons.OrdemServico os, Guid id) => ValidarOS(os, id);
        protected virtual void ValidarOSUpdate(Core.Entities.Domains.Commons.OrdemServico os, Guid id) => ValidarOS(os, id);
        protected virtual void ValidarOSDelete(Core.Entities.Domains.Commons.OrdemServico os, Guid id) => ValidarOS(os, id);

        protected virtual void ValidarOS(Core.Entities.Domains.Commons.OrdemServico os, Guid id)
            => os.Fail(os.Status != StatusOrdemServico.EmAberto && os.Status != StatusOrdemServico.EmAndamento && os.Status != StatusOrdemServico.EmPreenchimento,
                        new Error("Só é permitido editar ordens 'Em Preenchimento', 'Em Aberto' e 'Em Andamento'", "status"));

        private Core.Entities.Domains.Commons.OrdemServico GetOrdemServico(Guid id)
            => _ordemServicoBL.All.AsNoTracking().FirstOrDefault(e => e.Id == id);
    }
}
