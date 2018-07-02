using Fly01.Core.Helpers.Attribute;

namespace Fly01.Core.Entities.Domains.Enum
{
    public enum StatusContaBancaria
    {
        [Subtitle("EmAberto", "Em aberto", "ABER", "totvs-blue")]
        EmAberto = 1,

        [Subtitle("Pago", "Pago", "PAGO", "green")]
        Pago = 2,

        [Subtitle("Renegociado", "Renegociado", "RENG", "red")]
        Renegociado = 3,

        [Subtitle("BaixadoParcialmente", "Baixado Parcialmente", "BPAR", "gray")]
        BaixadoParcialmente = 4
    }
}
