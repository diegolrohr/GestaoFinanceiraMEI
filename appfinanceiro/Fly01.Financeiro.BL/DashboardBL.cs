using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Helpers;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.Financeiro.API.Models.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
                     x.ValorPrevisto,
                     VPg = (x.ValorPago ?? 0),
                     Dif = x.ValorPrevisto - (x.ValorPago ?? 0)
                 }).GroupBy(x => new { x.Status })
                 .Select(x => new
                 {
                     Tipo = x.Key.Status,
                     Total = x.Sum(v => v.ValorPrevisto),
                     Total2 = x.Sum(v => v.Dif),
                     Total3 = x.Sum(v => v.VPg),
                     Quantidade = x.Count()
                 }).ToList();

            double parcial = 0;
            int quantidadeParcial = 0;
            DashboardFinanceiroVM aberto = null;

            foreach (var item in contaFinanceira)
            {
                DashboardFinanceiroVM dashFinanceiro = new DashboardFinanceiroVM();
                dashFinanceiro.Tipo = EnumHelper.GetValue(typeof(StatusContaBancaria), item.Tipo.ToString());


                if (dashFinanceiro.Tipo == "Em aberto")
                {
                    dashFinanceiro.Quantidade = item.Quantidade;
                    dashFinanceiro.Total = item.Total2;
                    aberto = dashFinanceiro;
                }

                else if (dashFinanceiro.Tipo == "Pago")
                {
                    dashFinanceiro.Quantidade = item.Quantidade;
                    dashFinanceiro.Total = item.Total3;

                }

                else if (dashFinanceiro.Tipo == "Baixado Parcialmente")
                {
                    dashFinanceiro.Quantidade = item.Quantidade;
                    dashFinanceiro.Total = item.Total3;
                    parcial = item.Total - item.Total3;
                    quantidadeParcial = item.Quantidade;
                }

                DashboardFinanceiroLista.Add(dashFinanceiro);

            }
            
            if (parcial > 0)
            {
                if (aberto == null)
                {
                    aberto = new DashboardFinanceiroVM();
                    aberto.Tipo = EnumHelper.GetValue(typeof(StatusContaBancaria), StatusContaBancaria.EmAberto.ToString());
                    DashboardFinanceiroLista.Add(aberto);
                }
                aberto.Total += parcial;
                aberto.Quantidade += quantidadeParcial;

            }

            return DashboardFinanceiroLista;

        }

        public List<ContasReceberDoDiaVM> GetDashContasReceberPagoPorDia(DateTime filtro)
        {
            var mesAtual = CarregaMes(filtro.Month);
            return _contaFinanceiraBaixaBL.AllIncluding(x => x.ContaFinanceira).AsNoTracking().Where(x => x.Data.Month.Equals(filtro.Month) && x.Data.Year.Equals(filtro.Year) && x.ContaFinanceira.TipoContaFinanceira == TipoContaFinanceira.ContaReceber)
            .Select(x => new
            {
                x.Data.Day,
                x.Data.Month,
                x.Valor
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
                    x.ValorPago,
                    x.StatusContaBancaria
                }).OrderBy(x => x.DataVencimento).Skip(skipRecords).Take(takeRecords).ToList();

            foreach (var item in contaFinanceira)
            {
                ContasPagarDoDiaVM dashContaPagarDia = new ContasPagarDoDiaVM();
                dashContaPagarDia.Status = EnumHelper.GetValue(typeof(StatusContaBancaria), item.StatusContaBancaria.ToString());
                dashContaPagarDia.Valor = (item.ValorPrevisto - (item.ValorPago ?? 0));
                dashContaPagarDia.Vencimento = item.DataVencimento;
                dashContaPagarDia.Descricao = item.Descricao;
                dashLista.Add(dashContaPagarDia);
            }

            return dashLista;
        }

    }
}
