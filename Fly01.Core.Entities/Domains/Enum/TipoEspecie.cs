using Fly01.Core.Helpers.Attribute;

namespace Fly01.Core.Entities.Domains.Enum
{
    public enum TipoEspecie
    {
        [Subtitle("PASS", "Passageiro")]
        Passageiro = 1,

        [Subtitle("CARG", "Carga")]
        Carga = 2,

        [Subtitle("MIST", "Misto")]
        Misto = 3,

        [Subtitle("CORR", "Corrida")]
        Corrida = 4,

        [Subtitle("TRAC", "Tração")]
        Tracao = 5,

        [Subtitle("ESPE", "Espécie")]
        Especie = 6
    }
}