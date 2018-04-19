using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using Fly01.Financeiro.API.Models.DAL;
using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Entities.Domains.Enum;

namespace Fly01.Financeiro.BL
{
    public class FluxoCaixaBL : DomainBaseBL<FluxoCaixa>
    {
        private SaldoHistoricoBL saldoHistoricoBL;
        private ContaFinanceiraBL contaFinanceiraBL;

        public FluxoCaixaBL(AppDataContext context, SaldoHistoricoBL saldoHistoricoBL, ContaFinanceiraBL contaFinanceiraBL)
            : base(context)
        {
            this.saldoHistoricoBL = saldoHistoricoBL;
            this.contaFinanceiraBL = contaFinanceiraBL;
        }

        #region #1 Saldo de Todas as Contas (Consolidado) + AReceber e APagar (hoje)
        public FluxoCaixaSaldo GetSaldos()
        {
            var saldoTodasAsContas = saldoHistoricoBL.GetSaldos().FirstOrDefault(x => x.ContaBancariaId == Guid.Empty).SaldoConsolidado;
            var dataBase = DateTime.Now.Date;

            var contasFinanceirasBase = contaFinanceiraBL.All
                .Where(x => x.DataVencimento <= dataBase)
                .Where(x => x.StatusContaBancaria == StatusContaBancaria.EmAberto || x.StatusContaBancaria == StatusContaBancaria.BaixadoParcialmente)
                .Select(item => new
                {
                    TipoContaFinanceira = item.TipoContaFinanceira,
                    ValorPrevisto = item.ValorPrevisto,
                    ValorPago = item.ValorPago == null ? default(double) : (double)item.ValorPago
                });

            var totalAReceberHoje = contasFinanceirasBase
                .Where(x => x.TipoContaFinanceira == TipoContaFinanceira.ContaReceber)
                .AsNoTracking()
                .ToList()
                .Sum(x => x.ValorPrevisto - x.ValorPago);

            var totalAPagarHoje = contasFinanceirasBase
                .Where(x => x.TipoContaFinanceira == TipoContaFinanceira.ContaPagar)
                .AsNoTracking()
                .ToList()
                .Sum(x => x.ValorPrevisto - x.ValorPago);

            return new FluxoCaixaSaldo()
            {
                SaldoConsolidado = Math.Round(saldoTodasAsContas, 2),
                AReceberHoje = Math.Round(totalAReceberHoje, 2),
                APagarHoje = Math.Round(totalAPagarHoje, 2)
            };
        }
        #endregion

        #region #2 Projeção do Fluxo de Caixa
        public List<FluxoCaixaProjecao> GetProjecao(DateTime dataInicial, DateTime dataFinal)
        {
            var saldoInicial = saldoHistoricoBL.GetSaldos().FirstOrDefault(x => x.ContaBancariaId == Guid.Empty).SaldoConsolidado;

            var contasFinanceirasVencidas = contaFinanceiraBL.All
                .Where(x => x.StatusContaBancaria == StatusContaBancaria.EmAberto || x.StatusContaBancaria == StatusContaBancaria.BaixadoParcialmente)
                .Where(x => x.DataVencimento <= dataInicial)
                .Select(item => new
                {
                    Id = item.Id,
                    Data = dataInicial,
                    TipoContaFinanceira = item.TipoContaFinanceira,
                    ValorPrevisto = item.ValorPrevisto,
                    ValorPago = item.ValorPago == null ? default(double) : (double)item.ValorPago
                }).ToList();

            var contasFinanceirasPeriodo = contaFinanceiraBL.All
                .Where(x => x.StatusContaBancaria == StatusContaBancaria.EmAberto || x.StatusContaBancaria == StatusContaBancaria.BaixadoParcialmente)
                .Where(x => x.DataVencimento > dataInicial && x.DataVencimento <= dataFinal)
                .Select(item => new
                {
                    Id = item.Id,
                    Data = item.DataVencimento,
                    TipoContaFinanceira = item.TipoContaFinanceira,
                    ValorPrevisto = item.ValorPrevisto,
                    ValorPago = item.ValorPago == null ? default(double) : (double)item.ValorPago
                }).ToList();

            var allContasFinanceiras = contasFinanceirasVencidas.Union(contasFinanceirasPeriodo).OrderBy(x => x.Data).ThenBy(n => n.Id);

            var projecaoFluxoCaixa = (from cc in allContasFinanceiras
                                      group cc by cc.Data into g
                                      select new FluxoCaixaProjecao()
                                      {
                                          Data = g.Key,
                                          SaldoFinal = default(double), // (cumulativo: Calculado abaixo a partir do aggregator)
                                          TotalPagamentos = Math.Round(g.Where(x => x.TipoContaFinanceira == TipoContaFinanceira.ContaPagar).Sum(x => x.ValorPrevisto - x.ValorPago), 2),
                                          TotalRecebimentos = Math.Round(g.Where(x => x.TipoContaFinanceira == TipoContaFinanceira.ContaReceber).Sum(x => x.ValorPrevisto - x.ValorPago), 2)
                                      }).OrderBy(x => x.Data).ToList();

            // Calcula saldos cumulativos (por data):
            // Exemplo:
            // var allSaldos = new List<int> { 1, 3, 12, 19, 33 };
            // var aggregator =  { 1, 1+3, 4+12, 16+19, 35+33 }
            // var aggregator =  { 1, 4, 16, 35, 68 }

            var aggregator = new AggregatorSaldos() { SumSaldoConsolidado = saldoInicial };
            var aggregatorResult = projecaoFluxoCaixa.Aggregate(aggregator, (output, item) =>
            {
                output.SumSaldoConsolidado += (item.TotalRecebimentos - item.TotalPagamentos);
                output.SaldoConsolidado.Add(output.SumSaldoConsolidado);

                return output;
            });

            for (int i = 0; i <= projecaoFluxoCaixa.Count - 1; i++)
                projecaoFluxoCaixa[i].SaldoFinal = Math.Round(aggregatorResult.SaldoConsolidado[i], 2);

            return projecaoFluxoCaixa;
        }
        #endregion

        #region #3 Projeção por periodo do Fluxo de Caixa
        public FluxoCaixaProjecao GetAllNextDays(DateTime dataInicial, DateTime dataFinal)
        {
            var saldoInicial = saldoHistoricoBL.GetSaldos().FirstOrDefault(x => x.ContaBancariaId == Guid.Empty).SaldoConsolidado;

            var contasAReceber = contaFinanceiraBL.All
                .Where(x => x.TipoContaFinanceira == TipoContaFinanceira.ContaReceber &&
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


            var contasAPagar = contaFinanceiraBL.All
                .Where(x => x.TipoContaFinanceira == TipoContaFinanceira.ContaPagar &&
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

            var allContasFinanceiras = contasAPagar.Union(contasAReceber).OrderBy(x => x.Data).ThenBy( n => n.Id);

            var sumTotalPagar = allContasFinanceiras.Where(x => x.TipoContaFinanceira == TipoContaFinanceira.ContaPagar).Sum(x => x.ValorPrevisto - x.ValorPago);
            var sumTotalReceber = allContasFinanceiras.Where(x => x.TipoContaFinanceira == TipoContaFinanceira.ContaReceber).Sum(x => x.ValorPrevisto - x.ValorPago);


            var saldoTotalFinal = Math.Round(sumTotalReceber + (sumTotalPagar * -1) + saldoInicial, 2);

            return new FluxoCaixaProjecao()
            {
                TotalRecebimentos = sumTotalReceber,
                TotalPagamentos = sumTotalPagar,
                SaldoFinal = saldoTotalFinal
            };
        }
        #endregion
    }
}
