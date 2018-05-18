using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using Fly01.Financeiro.API.Models.DAL;
using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Entities.Domains.Enum;
using System.Globalization;

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
        public FluxoCaixaSaldo GetSaldos(DateTime dataFinal)
        {
            var saldoTodasAsContas = saldoHistoricoBL.GetSaldos().FirstOrDefault(x => x.ContaBancariaId == Guid.Empty).SaldoConsolidado;
            var dataBase = dataFinal;

            var contasFinanceirasBase = contaFinanceiraBL.All
                .Where(x => x.DataVencimento <= dataBase)
                .Where(x => x.StatusContaBancaria == StatusContaBancaria.EmAberto || x.StatusContaBancaria == StatusContaBancaria.BaixadoParcialmente)
                .Select(item => new
                {
                    ValorPago = item.ValorPago == null ? default(double) : (double)item.ValorPago,
                    item.TipoContaFinanceira,
                    item.ValorPrevisto
                });

            var totalAReceber = contasFinanceirasBase
                .Where(x => x.TipoContaFinanceira == TipoContaFinanceira.ContaReceber)
                .AsNoTracking()
                .ToList()
                .Sum(x => x.ValorPrevisto - x.ValorPago);

            var totalAPagar = contasFinanceirasBase
                .Where(x => x.TipoContaFinanceira == TipoContaFinanceira.ContaPagar)
                .AsNoTracking()
                .ToList()
                .Sum(x => x.ValorPrevisto - x.ValorPago);

            var saldoProjetado = totalAReceber + (totalAPagar * -1) + saldoTodasAsContas;

            return new FluxoCaixaSaldo()
            {
                SaldoAtual = Math.Round(saldoTodasAsContas, 2),
                TotalRecebimentos = Math.Round(totalAReceber, 2),
                TotalPagamentos = Math.Round(totalAPagar, 2),
                SaldoProjetado = Math.Round(saldoProjetado, 2)
            };
        }
        #endregion

        #region #2 Projeção do Fluxo de Caixa
        public List<FluxoCaixaProjecao> GetProjecao(DateTime dataInicial, DateTime dataFinal, DateGroupType groupType)
        {
            var saldoInicial = saldoHistoricoBL.GetSaldos().FirstOrDefault(x => x.ContaBancariaId == Guid.Empty).SaldoConsolidado;

            var contasFinanceirasVencidas = contaFinanceiraBL.All
                .Where(x => x.StatusContaBancaria == StatusContaBancaria.EmAberto || x.StatusContaBancaria == StatusContaBancaria.BaixadoParcialmente)
                .Where(x => x.DataVencimento <= dataInicial)
                .Select(item => new
                {
                    Data = dataInicial,
                    ValorPago = item.ValorPago == null ? default(double) : (double)item.ValorPago,
                    item.Id,
                    item.TipoContaFinanceira,
                    item.ValorPrevisto
                }).ToList();

            var contasFinanceirasPeriodo = contaFinanceiraBL.All
                .Where(x => x.StatusContaBancaria == StatusContaBancaria.EmAberto || x.StatusContaBancaria == StatusContaBancaria.BaixadoParcialmente)
                .Where(x => x.DataVencimento > dataInicial && x.DataVencimento <= dataFinal)
                .Select(item => new
                {
                    Data = item.DataVencimento,
                    ValorPago = item.ValorPago == null ? default(double) : (double)item.ValorPago,
                    item.Id,
                    item.TipoContaFinanceira,
                    item.ValorPrevisto
                }).ToList();

            var allContasFinanceiras = contasFinanceirasVencidas.Union(contasFinanceirasPeriodo)
                .OrderBy(x => x.Data).ThenBy(n => n.Id);

            var projecaoFluxoCaixa = (from cc in allContasFinanceiras
                                      group cc by cc.Data into g
                                      select new FluxoCaixaProjecao()
                                      {
                                          Label = g.Key.ToString("yyyy-MM-dd"),
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

            //var projecaoGroupped = projecaoFluxoCaixa;
            projecaoFluxoCaixa = ResolveGroup(projecaoFluxoCaixa, groupType);

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

        private List<FluxoCaixaProjecao> ResolveGroup(List<FluxoCaixaProjecao> items, DateGroupType groupType)
        {
            var defaultCulture = new CultureInfo("pt-BR");

            switch (groupType)
            {
                case DateGroupType.Day:
                default:
                    return items;
                //case DateGroupType.Week:
                    //return items.GroupBy(cc => new { month = cc.Data.Month, year = cc.Data.Year })
                    //    .Select(g => new FluxoCaixaProjecao()
                    //    {
                    //        Label = $"{g.Key.month}/{g.Key.year}",
                    //        TotalPagamentos = Math.Round(g.Sum(x => x.TotalPagamentos)),
                    //        TotalRecebimentos = Math.Round(g.Sum(x => x.TotalRecebimentos), 2)
                    //    }).ToList();
                case DateGroupType.Month:
                    return items.GroupBy(cc => new { cc.Data.Month, cc.Data.Year })
                        .Select(g => new FluxoCaixaProjecao() {
                            Label = $"{defaultCulture.DateTimeFormat.GetAbbreviatedMonthName(g.Key.Month)}/{g.Key.Year}",
                            TotalPagamentos = Math.Round(g.Sum(x => x.TotalPagamentos), 2),
                            TotalRecebimentos = Math.Round(g.Sum(x => x.TotalRecebimentos), 2)
                        }).ToList();
                case DateGroupType.Quarter:
                    return items.GroupBy(cc => new { Quarter = (cc.Data.Month - 1) / 3, cc.Data.Year })
                        .Select(g => new FluxoCaixaProjecao()
                        {
                            Label = $"Q{g.Key.Quarter + 1}/{g.Key.Year}",
                            TotalPagamentos = Math.Round(g.Sum(x => x.TotalPagamentos), 2),
                            TotalRecebimentos = Math.Round(g.Sum(x => x.TotalRecebimentos), 2)
                        }).ToList();
                case DateGroupType.Halfyear:
                    return items.GroupBy(cc => new { Halfyear = (cc.Data.Month - 1) / 6, cc.Data.Year })
                        .Select(g => new FluxoCaixaProjecao()
                        {
                            Label = $"{g.Key.Year + 1}/{g.Key.Halfyear}",
                            TotalPagamentos = Math.Round(g.Sum(x => x.TotalPagamentos), 2),
                            TotalRecebimentos = Math.Round(g.Sum(x => x.TotalRecebimentos), 2)
                        }).ToList();
                case DateGroupType.Year:
                    return items.GroupBy(cc => new { cc.Data.Year })
                        .Select(g => new FluxoCaixaProjecao() {
                            Label = $"{g.Key.Year}",
                            TotalPagamentos = Math.Round(g.Sum(x => x.TotalPagamentos), 2),
                            TotalRecebimentos = Math.Round(g.Sum(x => x.TotalRecebimentos), 2)
                        }).ToList();
            }
        }

        #endregion
    }
}
