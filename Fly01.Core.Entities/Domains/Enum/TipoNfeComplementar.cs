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

        [Subtitle("ComplIcms", "Complemento de ICMS")]
        ComplIcms = 3,

        [Subtitle("ComplIcmsST", "Complemento de ICMS ST")]
        ComplIcmsST = 4,

        [Subtitle("ComplIpi", "Complemento de IPI")]
        ComplIpi = 5

    }
}
