using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.ViewModels.Presentation.Commons;
using System.Linq;

namespace Fly01.Core.API
{
    public class ProdutoServicoBaseController : ApiBaseController
    {
        protected IQueryable<ProdutoServicoVM> GetProdutosServicos(string filtro, IQueryable<Produto> produtos, IQueryable<Servico> servicos)
        {
            if (!string.IsNullOrWhiteSpace(filtro))
            {
                produtos = produtos.Where(x =>
                    (
                          (x.Descricao.ToUpper().Contains(filtro.ToUpper())) ||
                          (x.CodigoProduto.ToUpper().Contains(filtro.ToUpper())) ||
                          (x.CodigoBarras.ToUpper().Contains(filtro.ToUpper()))
                      ) &&
                      (
                          (x.ObjetoDeManutencao == ObjetoDeManutencao.Nao)
                      )
                  );

                servicos = servicos.Where(x =>
                      (
                          (x.Descricao.ToUpper().Contains(filtro.ToUpper())) ||
                          (x.CodigoServico.ToUpper().Contains(filtro.ToUpper()))
                      )
                  );
            }

            var result = produtos.OrderBy(x => x.Descricao).Take(20)
                  .Select(y => new ProdutoServicoVM()
                  {
                      TipoItem = TipoItem.Produto,
                      Id = y.Id,
                      Descricao = y.Descricao,
                      Codigo = y.CodigoProduto,
                      ValorCusto = y.ValorCusto,
                      ValorVenda = y.ValorVenda
                  });

            var resultServicos = servicos.OrderBy(x => x.Descricao)
                  .Take(20)
                  .Select(y => new ProdutoServicoVM()
                  {
                      TipoItem = TipoItem.Servico,
                      Id = y.Id,
                      Descricao = y.Descricao,
                      Codigo = y.CodigoServico,
                      ValorCusto = y.ValorServico,
                      ValorVenda = y.ValorServico
                  });

            return result.Union(resultServicos).OrderBy(x => x.Descricao);
        }
    }
}
