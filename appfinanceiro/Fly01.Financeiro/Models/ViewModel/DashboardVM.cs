using System.Collections.Generic;

namespace Fly01.Financeiro.Models.ViewModel
{
    public class DashboardVM
    {
        public long ElapsedMilliseconds { get; set; }

        public DashboardSaldosVM DashboardSaldos { get; set; }

        public DashboardFluxoCaixaVM DashboardFluxoCaixa { get; set; }

        public DashboardVM()
        {
            DashboardSaldos = new DashboardSaldosVM(false);
            DashboardFluxoCaixa = new DashboardFluxoCaixaVM(false);
        }
    }

    #region DashboardSaldos
    public class DashboardSaldosVM
    {
        public DashboardSaldosVM(bool _render)
        {
            Render = _render;
        }

        public bool Render { get; set; }

        public string SaldoAtual { get; set; }

        public string TotalAReceberHoje { get; set; }

        public string TotalAPagarHoje { get; set; }
    }
    #endregion

    #region DashboardFluxoCaixa
    public enum DashboardFluxoCaixaType
    {
        Mensal = 1,

        PreDefinido = 2,

        Periodo = 3
    }

    public class DashboardFluxoCaixaVM
    {
        public DashboardFluxoCaixaVM(bool _render)
        {
            Render = _render;
        }

        public bool Render { get; set; }

        public DashboardFluxoCaixaType Type { get; set; }

        public List<DashboardFluxoCaixaItemVM> Items { get; set; }

        public string QueryStringGridFluxoCaixa { get; set; }
    }

    public class DashboardFluxoCaixaItemVM
    {
        public string Label { get; set; }

        public double Pagamentos { get; set; }

        public double Recebimentos { get; set; }

        public double Saldo { get; set; }
    }
    #endregion
}