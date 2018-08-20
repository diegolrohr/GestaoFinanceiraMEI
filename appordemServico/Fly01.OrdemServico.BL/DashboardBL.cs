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

            //var result = _ordemServicoBL.All.Where(x=>x.) 

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
        public List<string> GetTopServicos(DateTime filtro)
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
    }
}
