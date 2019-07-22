using System;
using System.Collections.Generic;
using System.Linq;
using Fly01.Financeiro.API.Models.DAL;
using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Notifications;
using Fly01.Core.Entities.Domains.Enum;

namespace Fly01.Financeiro.BL
{
    public class SaldoHistoricoBL : PlataformaBaseBL<SaldoHistorico>
    {
        private const string labelTodasAsContas = "Todas as Contas";

        public SaldoHistoricoBL(AppDataContext context)
            : base(context) { }

        public List<ExtratoContaSaldo> GetSaldos()
        {
            var saldos = (from si in AllIncluding(x => x.ContaBancaria)
                          where si.Ativo && si.ContaBancaria.Ativo
                          group si by si.ContaBancariaId into g
                          let dataRecord = g.OrderByDescending(t => t.Data).FirstOrDefault()
                          select new ExtratoContaSaldo()
                          {
                              ContaBancariaDescricao = dataRecord.ContaBancaria.NomeConta,
                              ContaBancariaId = dataRecord.ContaBancariaId,
                              SaldoConsolidado = Math.Round(dataRecord.SaldoConsolidado, 2)
                          }).ToList();

            saldos.Insert(0, new ExtratoContaSaldo()
            {
                ContaBancariaId = Guid.Empty,
                ContaBancariaDescricao = labelTodasAsContas,
                SaldoConsolidado = saldos.Sum(x => x.SaldoConsolidado)
            });

            return saldos;
        }

        public void InsereSaldoInicial(Guid contaBancariaId, double? valorInicial = 0.0)
        {
            if (contaBancariaId == default(Guid))
                throw new BusinessException("Conta Bancária Inválida");

            if (!All.Any(x => x.ContaBancariaId == contaBancariaId))
            {
                var saldoInicial = new SaldoHistorico()
                {
                    ContaBancariaId = contaBancariaId,
                    Data = DateTime.Now.Date,
                    SaldoDia = (double)valorInicial,
                    SaldoConsolidado = (double)valorInicial,
                    TotalPagamentos = default(double),
                    TotalRecebimentos = default(double),
                };

                base.Insert(saldoInicial);
            }
        }

        public void AtualizaSaldoHistorico(DateTime data, double valorBase, Guid contaBancariaId, TipoContaFinanceira tipoContaFinanceira)
        {
            if (contaBancariaId == default(Guid))
                throw new BusinessException("Conta Bancária Inválida");

            var valorContabil = valorBase;
            if (tipoContaFinanceira == TipoContaFinanceira.ContaPagar)
                valorContabil = valorBase * -1;

            var saldoHistorico = All.FirstOrDefault(x => x.ContaBancariaId == contaBancariaId && x.Data == data.Date);
            if (saldoHistorico == null)
            {
                var ultimoSaldo = All.OrderByDescending(x => x.Data).FirstOrDefault(x => x.ContaBancariaId == contaBancariaId && x.Data < data);

                var saldoConsolidado = default(double);
                if (ultimoSaldo != null)
                    saldoConsolidado = ultimoSaldo.SaldoConsolidado;

                saldoConsolidado += valorContabil;

                // Inclui Novo Saldo
                base.Insert(new SaldoHistorico()
                {
                    ContaBancariaId = contaBancariaId,
                    Data = data,
                    SaldoDia = valorContabil,
                    SaldoConsolidado = saldoConsolidado,
                    TotalPagamentos = tipoContaFinanceira == TipoContaFinanceira.ContaPagar ? valorBase : default(double),
                    TotalRecebimentos = tipoContaFinanceira == TipoContaFinanceira.ContaReceber ? valorBase : default(double)
                });
            }
            else
            {
                saldoHistorico.SaldoConsolidado += valorContabil;

                // Atualiza Saldo Existente
                switch (tipoContaFinanceira)
                {
                    case TipoContaFinanceira.ContaPagar:
                        saldoHistorico.TotalPagamentos += valorBase;
                        break;
                    case TipoContaFinanceira.ContaReceber:
                        saldoHistorico.TotalRecebimentos += valorBase;
                        break;
                }

                saldoHistorico.SaldoDia += valorContabil;

                Update(saldoHistorico);
            }
        }

        public void AtualizaSaldoHistorico(DateTime data, double valorBase, Guid contaBancariaId, TipoCarteira tipoCarteira)
        {
            if (contaBancariaId == default(Guid))
                throw new BusinessException("Conta Bancária Inválida");

            double valorContabil = valorBase;
            if (tipoCarteira == TipoCarteira.Despesa)
                valorContabil *= -1;

            SaldoHistorico saldoHistorico = All.FirstOrDefault(x => x.ContaBancariaId == contaBancariaId && x.Data == data.Date);
            if (saldoHistorico == null)
            {
                var ultimoSaldo = All.OrderByDescending(x => x.Data).FirstOrDefault(x => x.ContaBancariaId == contaBancariaId && x.Data < data);

                var saldoConsolidado = default(double);
                if (ultimoSaldo != null)
                    saldoConsolidado = ultimoSaldo.SaldoConsolidado;

                saldoConsolidado += valorContabil;

                // Inclui Novo Saldo
                var sal = new SaldoHistorico()
                {
                    ContaBancariaId = contaBancariaId,
                    Data = data,
                    SaldoDia = valorContabil,
                    SaldoConsolidado = saldoConsolidado,
                    TotalPagamentos = tipoCarteira == TipoCarteira.Despesa ? valorBase : default(double),
                    TotalRecebimentos = tipoCarteira == TipoCarteira.Receita ? valorBase : default(double)
                };
                base.Insert(sal);
            }
            else
            {
                saldoHistorico.SaldoConsolidado += valorContabil;

                // Atualiza Saldo Existente
                switch (tipoCarteira)
                {
                    case TipoCarteira.Despesa:
                        saldoHistorico.TotalPagamentos += valorBase;
                        break;
                    case TipoCarteira.Receita:
                        saldoHistorico.TotalRecebimentos += valorBase;
                        break;
                }

                saldoHistorico.SaldoDia += valorContabil;

                Update(saldoHistorico);
            }
        }
    }
}
