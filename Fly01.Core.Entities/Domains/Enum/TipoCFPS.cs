using Fly01.Core.Helpers.Attribute;

namespace Fly01.Core.Entities.Domains.Enum
{
    public enum TipoCFPS
    {
        [Subtitle("Tomador", "Para Tomador")]
        Tomador = 1,
        [Subtitle("Para3", "Para bem de 3º por conta do Tomador")]
        Para3 = 2
    }
}
