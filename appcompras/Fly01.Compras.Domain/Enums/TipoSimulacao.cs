using Fly01.Core.Entities.Attribute;

namespace Fly01.Compras.Domain.Enums
{
    public enum TipoSimulacao
    {
        [Subtitle("QtdParcelas", "Quantidade de parcelas")]
        QtdParcelas = 1,
        [Subtitle("CondParcelamento", "Cond. Parcelamento")]
        CondParcelamento = 2
    }
}