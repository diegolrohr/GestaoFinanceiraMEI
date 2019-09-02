using Fly01.Financeiro.API.Models.DAL;
using Fly01.Core.BL;
using System.Linq;
using System.Linq.Expressions;
using System;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Entities.Domains.Enum;

namespace Fly01.Financeiro.BL
{
    public class ContaFinanceiraBL : EmpresaBaseBL<ContaFinanceira>
    {
        private SaldoHistoricoBL saldoHistoricoBL;

        public ContaFinanceiraBL(AppDataContext context, SaldoHistoricoBL saldoHistoricoBL) 
            : base(context)
        {
            this.saldoHistoricoBL = saldoHistoricoBL;
        }

        public virtual IQueryable<ContaFinanceira> AllWithInactiveIncluding(params Expression<Func<ContaFinanceira, object>>[] includeProperties)
        {
            return repository.AllIncluding(includeProperties).Where(x => x.EmpresaId == EmpresaId);
        }
        
        public IQueryable<ContaFinanceira> Everything => repository.All.Where(x => x.Ativo);

        public FluxoCaixaProjecao GetAllContasNextDays(DateTime dataInicial, DateTime dataFinal, string plataformaId)
        {
            var saldoInicial = saldoHistoricoBL.GetSaldos().FirstOrDefault(x => x.ContaBancariaId == Guid.Empty).SaldoConsolidado;

            var contasReceber = Everything
                .Where(x => x.EmpresaId == EmpresaId && 
                            x.TipoContaFinanceira == TipoContaFinanceira.ContaReceber &&
                            (x.StatusContaBancaria == StatusContaBancaria.EmAberto || x.StatusContaBancaria == StatusContaBancaria.BaixadoParcialmente)
                 )
                .Where(x => x.DataVencimento <= dataFinal)
                .Select(item => new
                {
                    Id = item.Id,
                    Data = item.DataVencimento,
                    TipoContaFinanceira = item.TipoContaFinanceira,
                    ValorPrevisto = item.ValorPrevisto,
                    ValorPago = item.ValorPago == null ? default(double) : (double)item.ValorPago
                }).ToList();


            var contasAPagar = Everything
                .Where(x => x.EmpresaId == EmpresaId && 
                            x.TipoContaFinanceira == TipoContaFinanceira.ContaPagar &&
                            (x.StatusContaBancaria == StatusContaBancaria.EmAberto || x.StatusContaBancaria == StatusContaBancaria.BaixadoParcialmente)
                 )
                .Where(x => x.DataVencimento <= dataFinal)
                .Select(item => new
                {
                    Id = item.Id,
                    Data = item.DataVencimento,
                    TipoContaFinanceira = item.TipoContaFinanceira,
                    ValorPrevisto = item.ValorPrevisto,
                    ValorPago = item.ValorPago == null ? default(double) : (double)item.ValorPago
                }).ToList();

            var allContasFinanceiras = contasReceber.Union(contasAPagar).OrderBy(x => x.Data).ThenBy(n => n.Id);

            var sumTotalPagar = allContasFinanceiras.Where(x => x.TipoContaFinanceira == TipoContaFinanceira.ContaPagar).Sum(x => x.ValorPrevisto - x.ValorPago);
            var sumTotalReceber = allContasFinanceiras.Where(x => x.TipoContaFinanceira == TipoContaFinanceira.ContaReceber).Sum(x => x.ValorPrevisto - x.ValorPago);

            var saldoTotalFinal = Math.Round(sumTotalReceber + (sumTotalPagar * -1) + saldoInicial, 2);

            if (sumTotalPagar == default(int) && sumTotalReceber == default(int) && saldoTotalFinal == default(int))
                return null;

            return new FluxoCaixaProjecao()
            {
                TotalRecebimentos = sumTotalReceber,
                TotalPagamentos = sumTotalPagar,
                SaldoFinal = saldoTotalFinal
            };
        }
    }
}
