using Fly01.Core.Helpers.Attribute;

namespace Fly01.Core.Entities.Domains.Enum
{
    public enum TipoCodigoBanco
    {
        [Subtitle("BancoBrasil", "BancoBrasil")]
        BancoBrasil = 1,

        [Subtitle("Santander", "Santander")]
        Santander = 33,

        [Subtitle("Bradesco", "Bradesco")]
        Bradesco = 237,

        [Subtitle("Caixa", "Caixa")]
        Caixa = 104,

        [Subtitle("Itau", "Itau")]
        Itau = 341,

        [Subtitle("Banrisul", "Banrisul")]
        Banrisul = 41,
    }
}
