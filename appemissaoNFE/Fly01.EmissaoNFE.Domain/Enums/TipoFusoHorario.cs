using Fly01.Core.Helpers;
using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Enums
{
    public enum TipoFusoHorario
    {
        [XmlEnum(Name = "1")]
        [Subtitle("FernandoDeNoronha", "1", "Fernando de Noronha")]
        FernandoDeNoronha = 1,

        [XmlEnum(Name = "2")]
        [Subtitle("Brasilia", "2", "Brasília")]
        Brasilia = 2,

        [XmlEnum(Name = "3")]
        [Subtitle("Manaus", "3", "Manaus")]
        Manaus = 3,

        [XmlEnum(Name = "4")]
        [Subtitle("Acre", "4", "Acre")]
        Acre = 4
    }
}
