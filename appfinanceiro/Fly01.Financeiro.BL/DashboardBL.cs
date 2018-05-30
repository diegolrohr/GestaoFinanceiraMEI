using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Helpers;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.Financeiro.API.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fly01.Financeiro.BL
{
    public class DashboardBL
    {
        private ContaFinanceiraBL _contaFinanceiraBL;
        private ContaFinanceiraBaixaBL _contaFinanceiraBaixaBL;

        public DashboardBL(AppDataContext context, ContaFinanceiraBL contaFinanceiraBL, ContaFinanceiraBaixaBL contaFinanceiraBaixaBL)
        {
            this._contaFinanceiraBL = contaFinanceiraBL;
            this._contaFinanceiraBaixaBL = contaFinanceiraBaixaBL;
        }

        public List<DashboardFinanceiroVM> GetDashFinanceiroPorStatus(DateTime filtro, string tipo)
        {
            List<DashboardFinanceiroVM> DashboardFinanceiroLista = new List<DashboardFinanceiroVM>();

            var contaFinanceira = _contaFinanceiraBL.All.Where(x => x.TipoContaFinanceira.ToString() == tipo
            && x.DataVencimento.Month.Equals(filtro.Month) && x.DataVencimento.Year.Equals(filtro.Year))
                 .Select(x => new
                 {
                     Status = x.StatusContaBancaria,
                     x.ValorPrevisto
                 }).GroupBy(x => new { x.Status })
                 .Select(x => new
                 {
                     Tipo = x.Key.Status,
                     Total = x.Sum(v => v.ValorPrevisto),
                     Quantidade = x.Count()
                 }).ToList();

            foreach (var item in contaFinanceira)
            {
                DashboardFinanceiroVM dashFinanceiro = new DashboardFinanceiroVM();
                dashFinanceiro.Tipo = EnumHelper.GetDescription(typeof(StatusContaBancaria), item.Tipo.ToString());
                dashFinanceiro.Quantidade = item.Quantidade;
                dashFinanceiro.Total = item.Total;
                DashboardFinanceiroLista.Add(dashFinanceiro);
            }

            return DashboardFinanceiroLista;

        }

        public List<ContasReceberPagoPorDiaVM> GetDashContasReceberPagoPorDia(DateTime filtro)
        {
            var mesAtual = filtro.Month.ToString();
            return _contaFinanceiraBaixaBL.All.Where(x => x.Data.Month.Equals(filtro.Month) && x.Data.Year.Equals(filtro.Year)
                    && x.ContaFinanceira.TipoContaFinanceira == TipoContaFinanceira.ContaReceber)
                .Select(x => new
                {
                    x.Data.Day,
                    x.Data.Month,
                    x.Valor
                }).GroupBy(x => new { x.Day, x.Month })
                .Select(x => new ContasReceberPagoPorDiaVM
                {
                    Dia = x.Key.Day.ToString() + "/" + mesAtual,
                    Total = x.Sum(v => v.Valor)
                }).ToList();

        }

        public List<DashboardFinanceiroVM> GetDashFinanceiroFormasPagamento(DateTime filtro, string tipo)
        {
            List<DashboardFinanceiroVM> DashboardFinanceiroLista = new List<DashboardFinanceiroVM>();
            var contaFinanceira = _contaFinanceiraBL.All.Where(x => x.TipoContaFinanceira.ToString() == tipo
            && x.DataVencimento.Month.Equals(filtro.Month) && x.DataVencimento.Year.Equals(filtro.Year))
                 .Select(x => new
                 {
                     x.FormaPagamento.TipoFormaPagamento,
                     x.ValorPrevisto
                 }).GroupBy(x => new { x.TipoFormaPagamento })
                 .Select(x => new
                 {
                     Tipo = x.Key.TipoFormaPagamento,
                     Total = x.Sum(v => v.ValorPrevisto),
                     Quantidade = x.Count()
                 }).ToList();

            foreach (var item in contaFinanceira)
            {
                DashboardFinanceiroVM dashFinanceiro = new DashboardFinanceiroVM();
                dashFinanceiro.Tipo = EnumHelper.GetDescription(typeof(StatusContaBancaria), item.Tipo.ToString());
                dashFinanceiro.Quantidade = item.Quantidade;
                dashFinanceiro.Total = item.Total;
                DashboardFinanceiroLista.Add(dashFinanceiro);
            }

            return DashboardFinanceiroLista;
        }


        public List<DashboardFinanceiroVM> GetDashFinanceiroCategoria(DateTime filtro, string tipo)
        {
            return _contaFinanceiraBL.All.Where(x => x.TipoContaFinanceira.ToString() == tipo
            && x.DataVencimento.Month.Equals(filtro.Month) && x.DataVencimento.Year.Equals(filtro.Year))
            .Select(x => new
            {
                x.Categoria.Descricao,
                x.ValorPrevisto
            }).GroupBy(x => new { x.Descricao })
            .Select(x => new DashboardFinanceiroVM
            {
                Tipo = x.Key.Descricao,
                Total = x.Sum(v => v.ValorPrevisto),
                Quantidade = x.Count()
            }).ToList();

        }

        public int GetContasPagarDoDiaCount(DateTime filtro)
        {
            return _contaFinanceiraBL.All.Where(x => x.TipoContaFinanceira == TipoContaFinanceira.ContaPagar && x.StatusContaBancaria != StatusContaBancaria.Pago
             && x.DataVencimento.Month.Equals(filtro.Month) && x.DataVencimento.Year.Equals(filtro.Year))
                 .Select(x => new
                 {
                     x.DataVencimento,
                     x.Descricao,
                     x.ValorPrevisto,
                     x.StatusContaBancaria
                 }).Count();
        }

        public List<ContasPagarDoDiaVM> GetDashContasPagarDoDia(DateTime filtro, int skipRecords, int takeRecords)
        {
            List<ContasPagarDoDiaVM> dashLista = new List<ContasPagarDoDiaVM>();
            var contaFinanceira = _contaFinanceiraBL.All.Where(x => x.TipoContaFinanceira == TipoContaFinanceira.ContaPagar && x.StatusContaBancaria != StatusContaBancaria.Pago
            && x.DataVencimento.Month.Equals(filtro.Month) && x.DataVencimento.Year.Equals(filtro.Year))
                .Select(x => new
                {
                    x.DataVencimento,
                    x.Descricao,
                    x.ValorPrevisto,
                    x.StatusContaBancaria
                }).OrderBy(x => x.DataVencimento).Skip(skipRecords).Take(takeRecords).ToList();

            foreach (var item in contaFinanceira)
            {
                ContasPagarDoDiaVM dashContaPagarDia = new ContasPagarDoDiaVM();
                dashContaPagarDia.Status = EnumHelper.GetDescription(typeof(StatusContaBancaria), item.StatusContaBancaria.ToString());
                dashContaPagarDia.Valor = item.ValorPrevisto;
                dashContaPagarDia.Vencimento = item.DataVencimento.ToShortDateString().ToString();
                dashContaPagarDia.Descrição = item.Descricao;
                dashLista.Add(dashContaPagarDia);
            }

            return dashLista;
        }

    }
}
