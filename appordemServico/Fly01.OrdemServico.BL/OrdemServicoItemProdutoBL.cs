using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Notifications;
using System;

namespace Fly01.OrdemServico.BL
{
    public class OrdemServicoItemProdutoBL : PlataformaBaseBL<OrdemServicoItemProduto>
    {
        private readonly ProdutoBL _produtoBL;

        public OrdemServicoItemProdutoBL(AppDataContextBase context, ProdutoBL produtoBL) : base(context)
        {
            _produtoBL = produtoBL;
        }

        public override void ValidaModel(OrdemServicoItemProduto entity)
        {
            if (entity.ProdutoId == Guid.Empty)
                entity.Fail(entity.ProdutoId == Guid.Empty, new Error("Produto não informado", "produtoId"));
            else
                entity.Fail(!_produtoBL.Exists(entity.ProdutoId), new Error("Produto informado não existe", "produtoId"));
            entity.Fail(entity.Valor < 0, new Error("Valor não pode ser negativo", "valor"));
            entity.Fail(entity.Quantidade <= 0, new Error("Quantidade deve ser positiva", "quantidade"));
            entity.Fail(entity.Desconto < 0, new Error("Desconto não pode ser negativo", "desconto"));
            entity.Fail(entity.Desconto > (entity.Quantidade * entity.Valor), new Error("O Desconto não pode ser maior ao total bruto", "desconto"));

            base.ValidaModel(entity);
        }
    }
}
