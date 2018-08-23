using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Helpers;
using Fly01.Core.ViewModels.Presentation.Commons;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Fly01.OrdemServico.BL
{
    public class DashboardBL
    {
        private readonly DbContext _context;
        private readonly OrdemServicoBL _ordemServicoBL;
        private readonly OrdemServicoItemServicoBL _ordemServicoItemServicoBL;
        private readonly OrdemServicoItemProdutoBL _ordemServicoItemProdutoBL;

        public DashboardBL(DbContext context, OrdemServicoBL ordemServicoBL, OrdemServicoItemServicoBL ordemServicoItemServicoBL, OrdemServicoItemProdutoBL ordemServicoItemProdutoBL)
        {
            _context = context;
            _ordemServicoBL = ordemServicoBL;
            _ordemServicoItemServicoBL = ordemServicoItemServicoBL;
            _ordemServicoItemProdutoBL = ordemServicoItemProdutoBL;
        }

        public List<OrdemServicoPorStatusVM> GetOrdemServicoPorStatus(DateTime filtro)
        {
            List<OrdemServicoPorStatusVM> listaResult = new List<OrdemServicoPorStatusVM>();
            int QtdTotal = _ordemServicoBL.All.AsNoTracking().Where(x => x.DataEmissao.Month.Equals(filtro.Month) && x.DataEmissao.Year.Equals(filtro.Year)).Count();

            var result = _ordemServicoBL.All.AsNoTracking().Where(x => x.DataEmissao.Month.Equals(filtro.Month) && x.DataEmissao.Year.Equals(filtro.Year))
                                            .GroupBy(x => new { x.Status })
                                            .Select(x => new 
                                            {
                                                x.Key.Status,
                                                Quantidade = x.Count()
                                            })
                                            .ToList();


            result.ForEach(x =>
            {
                listaResult.Add(new OrdemServicoPorStatusVM
                {
                    Status = EnumHelper.GetValue(typeof(StatusOrdemServico), x.Status.ToString()),
                Quantidade = x.Quantidade,
                    QuantidadeTotal = QtdTotal
                });
            });
            return listaResult;
        }

        public List<OrdemServicosPorDiaVM> GetQuantidadeOrdemServicoPorDia(DateTime filtro)
        {
            return _ordemServicoBL.All.AsNoTracking().Where(x => x.Status == StatusOrdemServico.Concluido && x.DataEmissao.Month.Equals(filtro.Month) && x.DataEmissao.Year.Equals(filtro.Year))
                                            .GroupBy(x => x.DataEmissao.Day)
                                            .Select(x => new OrdemServicosPorDiaVM
                                            {
                                                Dia = x.Key,
                                                QuantidadeServicos = x.Count()
                                            })
                                            .OrderBy(x => x.Dia)
                                            .ToList();
        }

        public List<TopServicosProdutosOrdemServicoVM> GetTopProdutosOrdemServico(DateTime filtro)
        {
            return _ordemServicoItemProdutoBL.All.AsNoTracking()
                                            .Where(x => x.OrdemServico.DataEmissao.Month.Equals(filtro.Month) && x.OrdemServico.DataEmissao.Year.Equals(filtro.Year))
                                            .Select(x => new
                                            {
                                                x.ProdutoId,
                                                x.Produto.Descricao,
                                                x.Quantidade,
                                                x.Valor
                                            })
                                            .GroupBy(x => new { x.ProdutoId, x.Descricao })
                                            .Select(x => new TopServicosProdutosOrdemServicoVM
                                            {
                                                Id = x.Key.ProdutoId,
                                                Descricao = x.Key.Descricao,
                                                Quantidade = x.Sum(y => y.Quantidade),
                                                ValorTotal = x.Sum(y=>y.Valor)
                                            })
                                            .OrderByDescending(x => x.Quantidade)
                                            .Take(10).ToList();
        }

        public List<TopServicosProdutosOrdemServicoVM> GetTopServicosOrdemServico(DateTime filtro)
        {

            return _ordemServicoItemServicoBL.All.AsNoTracking()
                                            .Where(x => x.OrdemServico.DataEmissao.Month.Equals(filtro.Month) && x.OrdemServico.DataEmissao.Year.Equals(filtro.Year))
                                            .Select(x => new
                                            {
                                                x.ServicoId,
                                                x.Servico.Descricao,
                                                x.Quantidade,
                                                x.Valor
                                            })
                                            .GroupBy(x => new { x.ServicoId, x.Descricao })
                                            .Select(x => new TopServicosProdutosOrdemServicoVM
                                            {
                                                Id = x.Key.ServicoId,
                                                Descricao = x.Key.Descricao,
                                                Quantidade = x.Sum(y => y.Quantidade),
                                                ValorTotal = x.Sum(y=>y.Valor)

                                            })
                                            .OrderByDescending(x => x.Quantidade)
                                            .Take(10).ToList();
        }

    }
}
