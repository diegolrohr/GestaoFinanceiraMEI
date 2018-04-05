using Fly01.Core.Attribute;

namespace Fly01.Compras.Domain.Enums
{
    public enum TipoPagamentoImpostoISS
    {
        [Subtitle("DentroMunicipio", "Dentro do Município")]
        DentroMunicipio = 1,
        [Subtitle("ForaMunicípio", "Fora do Município")]
        ForaMunicípio = 2,
        [Subtitle("Isencao", "Isenção")]
        Isencao = 3,
        [Subtitle("Imune", "Imune")]
        Imune = 4
    }
}