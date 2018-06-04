using Fly01.Core.Helpers.Attribute;
using System.Xml.Serialization;

namespace Fly01.Core.Entities.Domains.Enum
{
    public enum HorarioVerao
    {
        [XmlEnum(Name = "1")]
        [Subtitle("Sim", "Sim", "Sim")]
        Sim = 1,

        [XmlEnum(Name = "2")]
        [Subtitle("Nao", "Não", "Não")]
        Nao = 2
    }
}
