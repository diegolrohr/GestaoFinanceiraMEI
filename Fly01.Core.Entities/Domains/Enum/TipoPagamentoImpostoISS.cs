using Fly01.Core.Entities.Attribute;

namespace Fly01.Core.Entities.Domains.Enum
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