using Fly01.Core.Attribute;

namespace Fly01.Financeiro.Entities.ViewModel
{
    public enum TipoContaFinanceiraVM
    {
        [Subtitle("ContaPagar", "Conta Pagar")]
        ContaPagar = 1,

        [Subtitle("ContaReceber", "Conta Receber")]
        ContaReceber = 2,

        [Subtitle("Transferencia", "Transferência")]
        Transferencia = 3
    }
}