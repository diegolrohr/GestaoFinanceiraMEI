using Fly01.Core.Helpers.Attribute;

namespace Fly01.Core.Entities.Domains.Enum
{
    public enum TipoNfeComplementar
    {
        [Subtitle("NaoComplementar", "Não Complementar")]
        NaoComplementar = 0,

        [Subtitle("ComplPrecoQtd", "Complemento de Preço/Quantidade")]
        ComplPrecoQtd = 1,

        [Subtitle("ComplIcms", "Complemento de ICMS")]
        ComplIcms = 2,

        [Subtitle("ComplIcmsST", "Complemento de ICMS ST")]
        ComplIcmsST = 3,

        [Subtitle("ComplIpi", "Complemento de IPI")]
        ComplIpi = 4
    }
}
