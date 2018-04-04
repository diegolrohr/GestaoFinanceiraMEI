using Fly01.Core.Helpers;
using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Enums
{
    public enum TipoCFOP
    {
        [XmlEnum(Name = "1")]
        [Subtitle("Entrada", "Entrada")]
        Entrada = 1,

        [XmlEnum(Name = "2")]
        [Subtitle("Saida", "Saída")]
        Saida = 2,
    }
}
