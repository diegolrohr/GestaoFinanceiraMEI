using System.Linq;
using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Notifications;

namespace Fly01.Faturamento.BL
{
    public class OrdemVendaProdutoBL : PlataformaBaseBL<OrdemVendaProduto>
    {
        public OrdemVendaProdutoBL(AppDataContextBase context) : base(context)
        {
        }

        public override void ValidaModel(OrdemVendaProduto entity)
        {
            //TO-DO VERIFICAR COMO MANTER ESSAS VALIDAÇÕES PARA OUTROS TIPOS DE NOTAS
            //entity.Fail(entity.Valor <= 0 && (entity.OrdemVenda.TipoVenda.Equals("Devolucao") || entity.OrdemVenda.TipoVenda.Equals("Normal")), new Error("Valor deve ser superior a zero", "valor"));
            //entity.Fail(entity.Quantidade <= 0 && (entity.OrdemVenda.TipoVenda.Equals("Devolucao") || entity.OrdemVenda.TipoVenda.Equals("Normal")), new Error("Quantidade deve ser superior a zero", "quantidade"));
            entity.Fail(entity.Desconto < 0, new Error("Desconto não pode ser negativo", "desconto"));
            //entity.Fail(entity.Desconto >= (entity.Quantidade * entity.Valor), new Error("O Desconto não pode ser maior ou igual ao total", "desconto"));
            //entity.Fail(entity.Total <= 0 && (entity.OrdemVenda.TipoVenda.Equals("Devolucao") || entity.OrdemVenda.TipoVenda.Equals("Normal")), new Error("O Total deve ser superior a zero", "total"));

            var jaExiste = All.Any(x => x.OrdemVendaId == entity.OrdemVendaId && x.ProdutoId == entity.ProdutoId && x.GrupoTributarioId == entity.GrupoTributarioId && x.Id != entity.Id);
            entity.Fail(jaExiste, new Error("Este produto com este grupo tributário já está adicionado"));

            base.ValidaModel(entity);
        }
    }
}