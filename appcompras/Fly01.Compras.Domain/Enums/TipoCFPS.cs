using Fly01.Core.Entities.Attribute;

namespace Fly01.Compras.Domain.Enums
{
    public enum TipoCFPS
    {
        [Subtitle("Tomador", "Para Tomador")]
        Tomador = 1,
        [Subtitle("Para3", "Para bem de 3º por conta do Tomador")]
        Para3 = 2
    }
}
