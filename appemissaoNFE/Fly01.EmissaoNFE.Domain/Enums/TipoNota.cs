using Fly01.Core.Helpers.Attribute;
using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Enums
{
    public enum TipoNota
    {
        [XmlEnum(Name = "0")]
        [Subtitle("Entrada", "0", "Entrada")]
        Entrada = 0,

        [XmlEnum(Name = "1")]
        [Subtitle("Saida", "1", "Saída")]
        Saida = 1
    }
}
