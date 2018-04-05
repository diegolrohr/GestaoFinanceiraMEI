using Fly01.Core.Attribute;

namespace Fly01.Compras.Domain.Enums
{
    public enum TipoContaFinanceira
    {
        [Subtitle("ContaPagar", "Conta Pagar")]
        ContaPagar = 1,
        [Subtitle("ContaReceber", "Conta Receber")]
        ContaReceber = 2,
        //Transferencia = 3
    }
}
