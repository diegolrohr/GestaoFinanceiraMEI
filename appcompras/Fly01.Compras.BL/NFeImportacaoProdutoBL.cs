using Fly01.Core.BL;
using Fly01.Compras.DAL;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Notifications;
using System;

namespace Fly01.Compras.BL
{
    public class NFeImportacaoProdutoBL : PlataformaBaseBL<NFeImportacaoProduto>
    {
        public NFeImportacaoProdutoBL(AppDataContext context) : base(context)
        {
        }
        public override void ValidaModel(NFeImportacaoProduto entity)
        {
            entity.Fail(entity.Valor < 0, new Error("Valor não pode ser negativo", "valor"));
            entity.Fail(entity.FatorConversao < 0, new Error("Fator de conversão não pode ser negativo", "fatorConversao"));
            entity.Fail(entity.Quantidade <= 0, new Error("Quantidade deve ser superior a zero", "quantidade"));
            entity.Fail(entity.ValorVenda < 0, new Error("Valor de venda não pode ser negativo", "valorVenda"));
            entity.Fail((entity.ProdutoId == null || entity.ProdutoId == default(Guid)) && !entity.NovoProduto, new Error("Vincule a um produto ou marque para cadastrar um novo", "produtoId"));
            base.ValidaModel(entity);
        }
    }
}