using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Notifications;
using System;
using System.Linq;

namespace Fly01.OrdemServico.BL
{
    public class OrdemServicoManutencaoBL : PlataformaBaseBL<OrdemServicoManutencao>
    {
        private readonly ProdutoBL _produtoBL;

        public OrdemServicoManutencaoBL(AppDataContextBase context, ProdutoBL produtoBL) : base(context)
        {
            _produtoBL = produtoBL;
        }

        public override void ValidaModel(OrdemServicoManutencao entity)
        {
            if (entity.ProdutoId == Guid.Empty)
                entity.Fail(entity.ProdutoId == Guid.Empty, new Error("Produto não informado", "produtoId"));
            else
            {
                var produto = _produtoBL.All.Where(x => x.ObjetoDeManutencao && x.Id == entity.ProdutoId)
                    .Select(x => new
                    {
                        x.Descricao,
                        x.ObjetoDeManutencao
                    }).FirstOrDefault();
                entity.Fail(produto == null, new Error("Produto informado não existe", "produtoId"));
                if (produto != null)
                    entity.Fail(!produto.ObjetoDeManutencao, new Error("Produto informado não é um objeto de manutenção. Caso queira utilizá-lo dessa forma, deve marcar em seu cadastro a opção 'Objeto de Manutenção'", "produtoId"));
            }
            entity.Fail(entity.Quantidade <= 0, new Error("Quantidade deve ser positiva", "quantidade"));

            base.ValidaModel(entity);
        }
    }
}
