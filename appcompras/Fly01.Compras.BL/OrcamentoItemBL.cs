using Fly01.Compras.DAL;
using Fly01.Compras.Domain.Entities;
using Fly01.Core.BL;
using Fly01.Core.Notifications;
using System.Linq;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Compras.BL
{
    public class OrcamentoItemBL : PlataformaBaseBL<OrcamentoItem>
    {
        public OrcamentoItemBL(AppDataContext context) : base(context)
        {
        }

        public override void ValidaModel(OrcamentoItem entity)
        {
            entity.Fail(entity.Valor <= 0, new Error("Valor deve ser superior a zero", "valor"));
            entity.Fail(entity.Quantidade <= 0, new Error("Quantidade deve ser superior a zero", "quantidade"));
            entity.Fail(entity.Desconto < 0, new Error("Desconto não pode ser negativo", "desconto"));
            entity.Fail(entity.Desconto >= (entity.Quantidade * entity.Valor), new Error("O Desconto não pode ser maior ou igual ao total", "desconto"));
            entity.Fail(entity.Total <= 0, new Error("O Total deve ser superior a zero", "total"));

            var jaExiste = All.Any(x => x.OrcamentoId == entity.OrcamentoId && x.ProdutoId == entity.ProdutoId && x.FornecedorId == entity.FornecedorId && x.Id != entity.Id);
            entity.Fail(jaExiste, new Error("Este produto com este fornecedor já está adicionado a este orçamento"));

            base.ValidaModel(entity);
        }
    }
}