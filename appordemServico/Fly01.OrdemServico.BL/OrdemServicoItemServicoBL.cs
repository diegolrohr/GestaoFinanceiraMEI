using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Notifications;
using System;

namespace Fly01.OrdemServico.BL
{
    public class OrdemServicoItemServicoBL : PlataformaBaseBL<OrdemServicoItemServico>
    {
        private readonly ServicoBL _servicoBL;

        public OrdemServicoItemServicoBL(AppDataContextBase context, ServicoBL servicoBL) : base(context)
        {
            _servicoBL = servicoBL;
        }

        public override void ValidaModel(OrdemServicoItemServico entity)
        {
            if (entity.ServicoId == Guid.Empty)
                entity.Fail(entity.ServicoId == Guid.Empty, new Error("Serviço não informado", "produtoId"));
            else
                entity.Fail(!_servicoBL.Exists(entity.ServicoId), new Error("Serviço informado não existe", "produtoId"));
            entity.Fail(entity.Valor < 0, new Error("Valor não pode ser negativo", "valor"));
            entity.Fail(entity.Quantidade <= 0, new Error("Quantidade deve ser positiva", "quantidade"));
            entity.Fail(entity.Desconto < 0, new Error("Desconto não pode ser negativo", "desconto"));
            entity.Fail(entity.Desconto > (entity.Quantidade * entity.Valor), new Error("O Desconto não pode ser maior ao total bruto", "desconto"));

            base.ValidaModel(entity);
        }
    }
}
