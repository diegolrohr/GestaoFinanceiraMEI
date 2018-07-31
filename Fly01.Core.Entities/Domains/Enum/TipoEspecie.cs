using Fly01.Core.Helpers.Attribute;

namespace Fly01.Core.Entities.Domains.Enum
{
    public enum TipoEspecie
    {
        [Subtitle("Nenhum", "Nenhum")]
        Nenhum = 0,

        [Subtitle("Passageiro", "Passageiro")]
        Passageiro = 1,

        [Subtitle("Carga", "Carga")]
        Carga = 2,

        [Subtitle("Misto", "Misto")]
        Misto = 3,

        [Subtitle("Corrida", "Corrida")]
        Corrida = 4,

        [Subtitle("Tracao", "Tração")]
        Tracao = 5,

        [Subtitle("Especial", "Especial")]
        Especial = 6
    }
}