using Fly01.Core.Helpers.Attribute;

namespace Fly01.EmissaoNFE.Domain.Enums
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