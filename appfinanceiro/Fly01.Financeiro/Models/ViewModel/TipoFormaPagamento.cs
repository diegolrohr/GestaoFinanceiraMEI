using System.ComponentModel;

namespace Fly01.Financeiro.Models.ViewModel
{
    public enum TipoFormaPagamento
    {
        [Description("Dinheiro")]
        Dinheiro = 1,
        [Description("Cheque")]
        Cheque = 2,
        [Description("Cartão de Crédito")]
        CartaoCredito = 3,
        [Description("Cartão de Débito")]
        CartaoDebito = 4,
        [Description("Transferência")]
        Transferencia = 5,
        [Description("Boleto")]
        Boleto = 6
    }
}