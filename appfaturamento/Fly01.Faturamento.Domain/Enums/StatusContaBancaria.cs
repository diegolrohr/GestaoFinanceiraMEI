using Fly01.Core.Helpers;

namespace Fly01.Faturamento.Domain.Enums
{
    public enum StatusContaBancaria
    {
        [Subtitle("EmAberto", "Em aberto", "ABER", "orange")]
        EmAberto = 1,

        [Subtitle("Pago", "Pago", "PAGO", "green")]
        Pago = 2,

        [Subtitle("Renegociado", "Renegociado", "RENG", "red")]
        Renegociado = 3,

        [Subtitle("BaixadoParcialmente", "Baixado Parcialmente", "BPAR", "gray")]
        BaixadoParcialmente = 4
    }
}
