using Fly01.Core.Helpers.Attribute;

namespace Fly01.Core.Entities.Domains.Enum
{
    public enum TipoNfeComplementar
    {
        [Subtitle("NaoComplementar", "Não Complementar")]
        NaoComplementar = 0,

        [Subtitle("ComplPreco", "Complemento de Preço")]
        ComplPreco = 1,

        [Subtitle("ComplQtd", "Complemento de Quantidade")]
        ComplQtd = 2,

        [Subtitle("ComplIcms", "Complemento de Impostos")]
        ComplIcms = 3
    }
}
