using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Faturamento.DAL;
using Fly01.Core.Notifications;
using System.Linq;
using Fly01.Core.Entities.Domains.Enum;

namespace Fly01.Faturamento.BL
{
    public class NFeProdutoBL : PlataformaBaseBL<NFeProduto>
    {
        public NFeProdutoBL(AppDataContext context) : base(context)
        {
        }

        public override void ValidaModel(NFeProduto entity)
        {
            entity.Fail(entity.Valor < 0, new Error("Valor não pode ser negativo", "valor"));
            entity.Fail(entity.Quantidade < 0 , new Error("Quantidade não pode ser negativa", "quantidade"));
            entity.Fail(entity.Desconto < 0, new Error("Desconto não pode ser negativo", "desconto"));
            entity.Fail(entity.Desconto >= (entity.Quantidade * entity.Valor) && entity.Desconto > 0, new Error("O Desconto não pode ser maior ou igual ao total", "desconto"));
            entity.Fail(entity.Total < 0 , new Error("O Total não pode ser negativo", "total"));
             
            var jaExiste = All.Any(x => x.NotaFiscalId == entity.NotaFiscalId && x.ProdutoId == entity.ProdutoId && x.GrupoTributarioId == entity.GrupoTributarioId && x.Id != entity.Id);
            entity.Fail(jaExiste, new Error("Este produto com este grupo tributário já está adicionado a esta nota fiscal"));

            base.ValidaModel(entity);
        }
    }
}