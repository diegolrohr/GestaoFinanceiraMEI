using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Notifications;
using Fly01.OrdemServico.BL.Base;
using Fly01.OrdemServico.BL.Extension;

namespace Fly01.OrdemServico.BL
{
    public class OrdemServicoManutencaoBL : OrdemServicoItemBLBase<OrdemServicoManutencao>
    {
        private readonly ProdutoBL _produtoBL;

        public OrdemServicoManutencaoBL(AppDataContextBase context, ProdutoBL produtoBL) : base(context)
        {
            _produtoBL = produtoBL;
        }

        public override void ValidaModel(OrdemServicoManutencao entity)
        {
            var produto = entity.ValidForeignKey(x => x.ProdutoId, "Produto", "produtoId", _produtoBL, x => new
            {
                x.Descricao,
                x.ObjetoDeManutencao
            });
            if (produto != null)
                entity.Fail(produto.ObjetoDeManutencao == ObjetoDeManutencao.Nao, new Error("Produto informado não é um objeto de manutenção. Caso queira utilizá-lo dessa forma, deve marcar em seu cadastro a opção 'Objeto de Manutenção'", "produtoId"));

            entity.Fail(entity.Quantidade <= 0, new Error("Quantidade deve ser positiva", "quantidade"));

            base.ValidaModel(entity);
        }
    }
}
