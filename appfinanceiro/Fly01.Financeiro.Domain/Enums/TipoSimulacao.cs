using Fly01.Core.Helpers;

namespace Fly01.Financeiro.Domain.Enums
{
    public enum TipoSimulacao
    {
        [Subtitle("QtdParcelas", "Quantidade de parcelas")]
        QtdParcelas = 1,
        [Subtitle("CondParcelamento", "Cond. Parcelamento")]
        CondParcelamento = 2
    }
}