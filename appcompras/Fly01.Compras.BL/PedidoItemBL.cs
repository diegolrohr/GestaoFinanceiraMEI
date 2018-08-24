using Fly01.Compras.DAL;
using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Notifications;
using System.Linq;

namespace Fly01.Compras.BL
{
    public class PedidoItemBL : PlataformaBaseBL<PedidoItem>
    {
        public PedidoItemBL(AppDataContext context) : base(context)
        {
        }

        public override void ValidaModel(PedidoItem entity)
        {
            entity.Fail(entity.Valor <= 0, new Error("Valor deve ser superior a zero", "valor"));
            entity.Fail(entity.Quantidade <= 0, new Error("Quantidade deve ser superior a zero", "quantidade"));
            entity.Fail(entity.Desconto < 0, new Error("Desconto não pode ser negativo", "desconto"));
            entity.Fail(entity.Desconto > (entity.Quantidade * entity.Valor), new Error("O Desconto não pode ser maior que o valor total do produto.", "desconto"));
            entity.Fail(entity.Total <= 0, new Error("O Total deve ser superior a zero", "total"));

            var jaExiste = All.Any(x => x.PedidoId == entity.PedidoId && x.ProdutoId == entity.ProdutoId && x.GrupoTributarioId == entity.GrupoTributarioId && x.Id != entity.Id);
            entity.Fail(jaExiste, new Error("Este produto já está adicionado a este pedido"));

            base.ValidaModel(entity);
        }
    }
}