using Fly01.Core.Helpers.Attribute;

namespace Fly01.Core.Entities.Domains.Enum
{
    public enum TipoSimulacao
    {
        [Subtitle("QtdParcelas", "Quantidade de parcelas")]
        QtdParcelas = 1,
        [Subtitle("CondParcelamento", "Cond. Parcelamento")]
        CondParcelamento = 2
    }
}