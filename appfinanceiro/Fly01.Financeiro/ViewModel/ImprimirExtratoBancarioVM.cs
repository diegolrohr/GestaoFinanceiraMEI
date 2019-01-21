using System;

namespace Fly01.Financeiro.ViewModel
{
    public class ImprimirExtratoBancarioVM
    {
        #region Conta Bancaria
        public string ContaBancariaId { get; set; }
        public string ContaBancariaDescricao { get; set; }
        public double SaldoConsolidado { get; set; }
        #endregion

        #region Extrato
        public DateTime? Data { get; set; }
        public string Lancamento { get; set; }
        public string ClienteFornecedor { get; set; }
        public string ContaBancaria { get; set; }
        public double Valor { get; set; }
        #endregion

        public DateTime? DataInicial { get; set; }
        public DateTime? DataFinal { get; set; }
    }
}