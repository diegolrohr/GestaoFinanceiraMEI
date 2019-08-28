using Fly01.Core.Helpers.Attribute;
using System.Xml.Serialization;

namespace Fly01.Core.Entities.Domains.Enum
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