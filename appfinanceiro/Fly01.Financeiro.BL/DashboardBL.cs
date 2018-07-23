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
                dashFinanceiro.Tipo = EnumHelper.GetValue(typeof(StatusContaBancaria), item.Tipo.ToString());
                dashFinanceiro.Quantidade = item.Quantidade;
                dashFinanceiro.Total = item.Total;
                DashboardFinanceiroLista.Add(dashFinanceiro);
            }

            return DashboardFinanceiroLista;

        }

        public List<ContasReceberDoDiaVM> GetDashContasReceberPagoPorDia(DateTime filtro)
        {
            var mesAtual = CarregaMes(filtro.Month);
            return _contaFinanceiraBL.All.Where(x => x.DataVencimento.Month.Equals(filtro.Month) && x.DataVencimento.Year.Equals(filtro.Year)
                    && x.TipoContaFinanceira == TipoContaFinanceira.ContaReceber && x.ValorPago.HasValue)
                .Select(x => new
                {
                    x.DataVencimento.Day,
                    x.DataVencimento.Month,
                    Valor = x.ValorPago == null ? 0 : x.ValorPago
                }).GroupBy(x => new { x.Day, x.Month })
                .Select(x => new ContasReceberDoDiaVM
                {
                    Dia = x.Key.Day.ToString() + "/" + mesAtual,
                    Total = x.Sum(v => v.Valor)
                }).ToList();

        }

        public String CarregaMes(int mes)
        {
            switch (mes)
            {
                case 1:
                    return "Jan";
                    break;
                case 2:
                    return "Fev";
                    break;
                case 3:
                    return "Mar";
                    break;
                case 4:
                    return "Abr";
                    break;
                case 5:
                    return "Mai";
                    break;
                case 6:
                    return "Jun";
                    break;
                case 7:
                    return "Jul";
                    break;
                case 8:
                    return "Ago";
                    break;
                case 9:
                    return "Set";
                    break;
                case 10:
                    return "Out";
                    break;
                case 11:
                    return "Nov";
                    break;
                case 12:
                    return "Dez";
                    break;
                default:
                    return "";
                    break;
            }
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
                dashFinanceiro.Tipo = EnumHelper.GetValue(typeof(TipoFormaPagamento), item.Tipo.ToString());
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
                dashContaPagarDia.Status = EnumHelper.GetValue(typeof(StatusContaBancaria), item.StatusContaBancaria.ToString());
                dashContaPagarDia.Valor = item.ValorPrevisto;
                dashContaPagarDia.Vencimento = item.DataVencimento;
                dashContaPagarDia.Descricao = item.Descricao;
                dashLista.Add(dashContaPagarDia);
            }

            return dashLista;
        }

    }
}
