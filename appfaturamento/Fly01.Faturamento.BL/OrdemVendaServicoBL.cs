using Fly01.Core.Api.BL;
using Fly01.Faturamento.Domain.Entities;
using Fly01.Core;
using Fly01.Core.Notifications;
using System.Linq;

namespace Fly01.Faturamento.BL
{
    public class OrdemVendaServicoBL : PlataformaBaseBL<OrdemVendaServico>
    {
        public OrdemVendaServicoBL(AppDataContextBase context) : base(context)
        {
        }

        public override void ValidaModel(OrdemVendaServico entity)
        {
            entity.Fail(entity.Valor <= 0, new Error("Valor deve ser superior a zero", "valor"));
            entity.Fail(entity.Quantidade <= 0, new Error("Quantidade deve ser superior a zero", "quantidade"));
            entity.Fail(entity.Desconto < 0, new Error("Desconto não pode ser negativo", "desconto"));
            entity.Fail(entity.Desconto >= (entity.Quantidade * entity.Valor), new Error("O Desconto não pode ser maior ou igual ao total", "desconto"));
            entity.Fail(entity.Total <= 0, new Error("O Total deve ser superior a zero", "total"));

            var jaExiste = All.Any(x => x.OrdemVendaId == entity.OrdemVendaId && x.ServicoId == entity.ServicoId && x.GrupoTributarioId == entity.GrupoTributarioId && x.Id != entity.Id);
            entity.Fail(jaExiste, new Error("Este serviço com este grupo tributário já está adicionado"));

            base.ValidaModel(entity);
        }
    }
}