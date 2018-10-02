using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Notifications;
using Fly01.OrdemServico.BL.Base;
using Fly01.OrdemServico.BL.Extension;
using System;
using System.Data.Entity;
using System.Linq;

namespace Fly01.OrdemServico.BL
{
    public class OrdemServicoItemServicoBL : OrdemServicoItemBLBase<OrdemServicoItemServico>
    {
        private readonly ServicoBL _servicoBL;

        public OrdemServicoItemServicoBL(AppDataContextBase context, ServicoBL servicoBL) : base(context)
        {
            _servicoBL = servicoBL;
        }

        public override void ValidaModel(OrdemServicoItemServico entity)
        {
            entity.ValidForeignKey(x => x.ServicoId, "Servico", "servicoId", _servicoBL);
            entity.Fail(entity.Valor < 0, new Error("Valor não pode ser negativo", "valor"));
            entity.Fail(entity.Quantidade <= 0, new Error("Quantidade deve ser positiva", "quantidade"));
            entity.Fail(entity.Desconto < 0, new Error("Desconto não pode ser negativo", "desconto"));
            entity.Fail(entity.Desconto > (entity.Quantidade * entity.Valor), new Error("O Desconto não pode ser maior ao total bruto", "desconto"));

            base.ValidaModel(entity);
        }

        protected override void ValidarOSDelete(OrdemServicoItemServico entity, Core.Entities.Domains.Commons.OrdemServico os, Guid id)
        {
            base.ValidarOSDelete(entity, os, id);
            if (os.Status == StatusOrdemServico.EmAberto || os.Status == StatusOrdemServico.EmAndamento)
                entity.Fail(!All.AsNoTracking().Any(x => x.OrdemServicoId == entity.OrdemServicoId && x.Id != id), new Error("É preciso existir ao menos um serviço na ordem", "status"));
        }
    }
}
