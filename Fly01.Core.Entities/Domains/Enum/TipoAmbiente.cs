using Fly01.Core.Helpers.Attribute;
using System.Xml.Serialization;

namespace Fly01.Core.Entities.Domains.Enum
{
    public enum TipoAmbiente
    {
        [XmlEnum(Name = "0")]
        [Subtitle("Configuracao", "Configuração", "Configuração")]
        Configuracao = 0,

        [XmlEnum(Name = "1")]
        [Subtitle("Producao", "Produção", "Produção")]
        Producao = 1,

        [XmlEnum(Name = "2")]
        [Subtitle("Homologacao", "Homologação", "Homologação")]
        Homologacao = 2
    }
}
