using Fly01.Financeiro.Entities.Helpers;

namespace Fly01.Financeiro.Models.ViewModel
{
    public enum StatusContaBancaria
    {
        [Subtitle("fly-orange", "Em aberto", "ABER")]
        EmAberto = 1,

        [Subtitle("fly-green", "Baixado", "BAIX")]
        Pago = 2,

        [Subtitle("fly-red", "Renegociado", "RENG")]
        Renegociado = 3,

        [Subtitle("fly-gray", "Baixado Parcialmente", "BPAR")]
        BaixadoParcialmente = 4
    }
}