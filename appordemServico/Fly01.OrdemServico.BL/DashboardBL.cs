using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Commons;
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
        private readonly ProdutoBL _produtoBL;
        

        public DashboardBL(DbContext context, OrdemServicoBL ordemServicoBL, ProdutoBL produtoBL)
        {
            _context = context;
            _ordemServicoBL = ordemServicoBL;
            _produtoBL = produtoBL;
        }

        // criar vm em commons
        public List<string> GetOrdemServicoPorStatus(DateTime filtro, string status)
        {
            //var mesAtual = CarregaMes(filtro.Month);
            //return _contaFinanceiraBL.All.Where(x => x.DataVencimento.Month.Equals(filtro.Month) && x.DataVencimento.Year.Equals(filtro.Year)
            //        && x.TipoContaFinanceira == TipoContaFinanceira.ContaReceber && x.ValorPago.HasValue)
            //    .Select(x => new
            //    {
            //        x.DataVencimento.Day,
            //        x.DataVencimento.Month,
            //        Valor = x.ValorPago == null ? 0 : x.ValorPago
            //    }).GroupBy(x => new { x.Day, x.Month })
            //    .Select(x => new ContasReceberDoDiaVM
            //    {
            //        Dia = x.Key.Day.ToString() + "/" + mesAtual,
            //        Total = x.Sum(v => v.Valor)
            //    }).ToList();
            return null;
        }

        public int GetQuantidadeOrdemServicoPorDia(DateTime filtro)
        {
            //return _contaFinanceiraBL.All.Where(x => x.TipoContaFinanceira == TipoContaFinanceira.ContaPagar && x.StatusContaBancaria != StatusContaBancaria.Pago
            // && x.DataVencimento.Month.Equals(filtro.Month) && x.DataVencimento.Year.Equals(filtro.Year))
            //     .Select(x => new
            //     {
            //         x.DataVencimento,
            //         x.Descricao,
            //         x.ValorPrevisto,
            //         x.StatusContaBancaria
            //     }).Count();

            //var result = _ordemServicoBL.All.Where(x=>x.DataEmissao.Month.Equals(filtro.Month) && x.DataEmissao.Year.Equals(filtro.Year))
            //            .Select(x=>new {
                            
            //            })

            return 0;
        }

        // criar vm em commons
        public List<string> GetTopProdutosOrdemServico(DateTime filtro)
        {
            //return _contaFinanceiraBL.All.Where(x => x.TipoContaFinanceira == TipoContaFinanceira.ContaPagar && x.StatusContaBancaria != StatusContaBancaria.Pago
            // && x.DataVencimento.Month.Equals(filtro.Month) && x.DataVencimento.Year.Equals(filtro.Year))
            //     .Select(x => new
            //     {
            //         x.DataVencimento,
            //         x.Descricao,
            //         x.ValorPrevisto,
            //         x.StatusContaBancaria
            //     }).Count();
            return null;
        }

        // criar vm em commons
        public List<string> GetTopServicosOrdemServico(DateTime filtro)
        {
            //return _contaFinanceiraBL.All.Where(x => x.TipoContaFinanceira == TipoContaFinanceira.ContaPagar && x.StatusContaBancaria != StatusContaBancaria.Pago
            // && x.DataVencimento.Month.Equals(filtro.Month) && x.DataVencimento.Year.Equals(filtro.Year))
            //     .Select(x => new
            //     {
            //         x.DataVencimento,
            //         x.Descricao,
            //         x.ValorPrevisto,
            //         x.StatusContaBancaria
            //     }).Count();


            
                //.Where(x => x.Data.Month.Equals(filtro.Month) && x.Data.Year.Equals(filtro.Year) && x.Status == StatusOrdemCompra.Finalizado)
                //.Select(x => new
                //{
                //    x.Fornecedor.Id,
                //    x.Fornecedor.Nome,
                //    x.Total
                //})
                //.GroupBy(x => new { x.Id, x.Nome })
                //.Select(x => new MaioresFornecedoresVM
                //{
                //    Id = x.Key.Id,
                //    Nome = x.Key.Nome,
                //    Valor = x.Sum(y => y.Total ?? 0)
                //})
                //.OrderByDescending(x => x.Valor)
                //.Take(10).ToList();

            // retorna lista de id, nome e quantidade

            return null;
        }
    }
}
