using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using Fly01.Financeiro.API.Models.DAL;
using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Financeiro.BL
{
    public class ExtratoBL : DomainBaseBL<Extrato>
    {
        private SaldoHistoricoBL saldoHistoricoBL;
        private ContaBancariaBL contaBancariaBL;
        private MovimentacaoBL movimentacaoBL;

        private const string labelTodasAsContas = "Todas as Contas";

        public ExtratoBL(AppDataContext context, SaldoHistoricoBL saldoHistoricoBL, ContaBancariaBL contaBancariaBL, MovimentacaoBL movimentacaoBL)
            : base(context)
        {
            this.saldoHistoricoBL = saldoHistoricoBL;
            this.contaBancariaBL = contaBancariaBL;
            this.movimentacaoBL = movimentacaoBL;
        }

        #region #1 Saldos Consolidados por Conta
        public List<ExtratoContaSaldo> GetSaldos()
        {
            return saldoHistoricoBL.GetSaldos();
        }
        #endregion

        #region #2 Historico Saldos
        public ExtratoHistoricoSaldo GetHistoricoSaldos(DateTime dataInicial, DateTime dataFinal, Guid? contaBancariaId)
        {
            var dataSaldoInicial = dataInicial.AddDays(-1);
            var saldosIniciais = (from si in saldoHistoricoBL.All
                                  where si.Data < dataInicial.Date && si.ContaBancariaId == (contaBancariaId ?? si.ContaBancariaId)
                                  group si by si.ContaBancariaId into g
                                  let dataRecord = g.OrderByDescending(t => t.Data).FirstOrDefault()
                                  select new
                                  {
                                      ContaBancariaId = g.Key,
                                      Data = dataSaldoInicial,
                                      SaldoConsolidado = dataRecord.SaldoConsolidado,
                                      TotalPagamentos = dataInicial == dataRecord.Data ? dataRecord.TotalPagamentos : default(double),
                                      TotalRecebimentos = dataInicial == dataRecord.Data ? dataRecord.TotalRecebimentos : default(double),
                                      SaldoDia = dataRecord.SaldoDia
                                  }).ToList();

            var saldoInicial = saldosIniciais.Sum(e => e.SaldoConsolidado);
            var saldosIniciaisAny = saldosIniciais.Any();

            var saldosPeriodo = (from sp in saldoHistoricoBL.All
                                 where sp.Data >= dataInicial && sp.Data <= dataFinal && sp.ContaBancariaId == (contaBancariaId ?? sp.ContaBancariaId)
                                 select new
                                 {
                                     ContaBancariaId = sp.ContaBancariaId,
                                     Data = sp.Data,
                                     SaldoConsolidado = sp.SaldoConsolidado,
                                     TotalPagamentos = sp.TotalPagamentos,
                                     TotalRecebimentos = sp.TotalRecebimentos,
                                     SaldoDia = sp.SaldoDia
                                 }).ToList();

            var listOfBalances = saldosIniciais.Union(saldosPeriodo).Select(itemSaldo => new ExtratoSaldoHistoricoItem()
            {
                Data = itemSaldo.Data,
                SaldoConsolidado = Math.Round(itemSaldo.SaldoConsolidado, 2),
                SaldoDia = Math.Round(itemSaldo.SaldoDia, 2),
                TotalPagamentos = Math.Round(itemSaldo.TotalPagamentos, 2),
                TotalRecebimentos = Math.Round(itemSaldo.TotalRecebimentos, 2)
            }).OrderBy(x => x.Data).ToList();

            if (!listOfBalances.Any(x => x.Data == dataFinal))
            {
                var lastRecord = listOfBalances.LastOrDefault();

                if (lastRecord != null)
                {
                    listOfBalances.Add(new ExtratoSaldoHistoricoItem()
                    {
                        Data = dataFinal,
                        SaldoConsolidado = lastRecord.SaldoConsolidado,
                        SaldoDia = default(double),
                        TotalPagamentos = default(double),
                        TotalRecebimentos = default(double)
                    });
                }
            }

            if (contaBancariaId.HasValue)
                return GetHistoricoSaldosByConta(listOfBalances, (Guid)contaBancariaId);

            return GetHistoricoSaldosByAll(listOfBalances, saldoInicial, saldosIniciaisAny);
        }

        private ExtratoHistoricoSaldo GetHistoricoSaldosByConta(List<ExtratoSaldoHistoricoItem> listOfBalances, Guid contaBancariaId)
        {
            var contaBancaria = contaBancariaBL.All.Where(x => x.Id == contaBancariaId).Select(x => new { x.NomeConta }).FirstOrDefault();

            return new ExtratoHistoricoSaldo()
            {
                ContaBancariaId = contaBancariaId,
                ContaBancariaDescricao = contaBancaria != null
                    ? contaBancaria.NomeConta
                    : string.Empty,
                Saldos = listOfBalances
            };
        }

        private ExtratoHistoricoSaldo GetHistoricoSaldosByAll(List<ExtratoSaldoHistoricoItem> listOfBalances, double saldoInicial, bool saldosIniciaisAny = false)
        {
            // Agrupamento por Data
            listOfBalances = (from s in listOfBalances
                              group s by s.Data into g
                              select new ExtratoSaldoHistoricoItem()
                              {
                                  Data = g.Key,
                                  SaldoConsolidado = listOfBalances.FirstOrDefault().SaldoConsolidado, //(cumulativo de todas as contas e calculado com base no saldo do dia)
                                  SaldoDia = Math.Round(g.Sum(k => k.SaldoDia), 2),
                                  TotalPagamentos = Math.Round(g.Sum(k => k.TotalPagamentos), 2),
                                  TotalRecebimentos = Math.Round(g.Sum(k => k.TotalRecebimentos), 2)
                              }).ToList();

            // Calcula saldos cumulativos (por data):
            // Exemplo:
            // var allSaldos = new List<int> { 1, 3, 12, 19, 33 };
            // var aggregator =  { 1, 1+3, 4+12, 16+19, 35+33 }
            // var aggregator =  { 1, 4, 16, 35, 68 }

            var cont = 0;
            var aggregator = new AggregatorSaldos();
            var aggregatorResult = listOfBalances.Aggregate(aggregator, (output, item) =>
            {
                output.SumSaldoConsolidado += item.SaldoDia;

                if (cont < 1)
                {
                    //o saldo inicial pode estar zerado, por não ter datas anteriores ou pois a soma das datas anteriores podem ter se anuladas(zeradas +50 -50)
                    output.SumSaldoConsolidado = (saldoInicial != 0 && saldosIniciaisAny) ? saldoInicial : output.SumSaldoConsolidado;
                }

                output.SaldoConsolidado.Add(output.SumSaldoConsolidado);

                cont++;
                return output;
            });

            for (int i = 0; i <= listOfBalances.Count - 1; i++)
                listOfBalances[i].SaldoConsolidado = Math.Round(aggregatorResult.SaldoConsolidado[i], 2);

            return new ExtratoHistoricoSaldo()
            {
                ContaBancariaId = Guid.Empty,
                ContaBancariaDescricao = labelTodasAsContas,
                Saldos = listOfBalances
            };
        }
        #endregion

        #region #3 Detalhes das Movimentações
        public List<ExtratoDetalhe> GetExtratoDetalhe(DateTime dataInicial, DateTime dataFinal, Guid? contaBancariaId, int skipRecords, int takeRecords)
        {
            var contasBancarias = contaBancariaBL.All.Select(x => new { x.Id, x.NomeConta }).ToList();

            var movimentacoes = (from mov in movimentacaoBL.AllIncluding(x => x.ContaFinanceira, x => x.ContaFinanceira.Pessoa, x => x.ContaBancariaDestino, x => x.ContaBancariaOrigem)
                                 where (mov.Data >= dataInicial && mov.Data <= dataFinal) && mov.Ativo &&
                                 //((mov.ContaFinanceira != null && (mov.ContaFinanceira.Ativo && mov.ContaFinanceira.Pessoa.Ativo)) || (mov.ContaFinanceira == null)) &&
                                 (mov.ContaBancariaDestino.Ativo || mov.ContaBancariaOrigem.Ativo) &&
                                 (
                                     (contaBancariaId.HasValue) ?
                                     (mov.ContaBancariaDestinoId == contaBancariaId) || (mov.ContaBancariaOrigemId == contaBancariaId) :
                                     (mov.ContaBancariaDestino == null) || (mov.ContaBancariaOrigemId == null)
                                 )
                                 select new ExtratoDetalhe()
                                 {
                                     ContaBancariaId = (Guid)(mov.ContaBancariaDestinoId ?? mov.ContaBancariaOrigemId),
                                     ContaBancariaDescricao = string.Empty,
                                     DataMovimento = mov.Data,
                                     DataInclusao = mov.DataInclusao,
                                     DescricaoLancamento = mov.Descricao == null ? (mov.ContaFinanceira != null ? mov.ContaFinanceira.Descricao : "") : mov.Descricao,
                                     ContaFinanceiraNumero = mov.ContaFinanceira.Numero.ToString(),
                                     PessoaNome = mov.ContaFinanceira.Pessoa.Nome,
                                     ValorLancamento = Math.Round(mov.Valor, 2),
                                 }).OrderByDescending(x => x.DataMovimento).ThenByDescending(x => x.DataInclusao).Skip(skipRecords).Take(takeRecords).ToList();

            movimentacoes.ForEach(item =>
                item.ContaBancariaDescricao = contasBancarias.FirstOrDefault(x => x.Id == item.ContaBancariaId).NomeConta ?? item.DescricaoLancamento
            );

            return movimentacoes;
        }

        public int GetExtratoDetalheCount(DateTime dataInicial, DateTime dataFinal, Guid? contaBancariaId)
        {
            var countRecords = movimentacaoBL.AllIncluding(x => x.ContaFinanceira, x => x.ContaFinanceira.Pessoa, x => x.ContaBancariaDestino, x => x.ContaBancariaOrigem)
                .Count(x => x.Data >= dataInicial && x.Data <= dataFinal && x.Ativo &&
                //(x.ContaFinanceira != null && (x.ContaFinanceira.Ativo && x.ContaFinanceira.Pessoa.Ativo) || x.ContaFinanceira == null) &&
                (x.ContaBancariaDestino.Ativo || x.ContaBancariaOrigem.Ativo) &&
                (
                    (contaBancariaId.HasValue)?
                    (x.ContaBancariaDestinoId == contaBancariaId) || (x.ContaBancariaOrigemId == contaBancariaId):
                    (x.ContaBancariaDestino == null) || (x.ContaBancariaOrigem == null)
                )
            );

            return countRecords;
        }

        #endregion
    }
}