using Fly01.Core.Attribute;

namespace Fly01.Faturamento.Domain.Enums
{
    public enum TipoCFPS
    {
        [Subtitle("Tomador", "Para Tomador")]
        Tomador = 1,
        [Subtitle("Para3", "Para bem de 3º por conta do Tomador")]
        Para3 = 2
    }
}
