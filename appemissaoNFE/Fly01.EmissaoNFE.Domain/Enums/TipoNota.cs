using Fly01.Core.Helpers.Attribute;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TipoNota
    {
        [XmlEnum(Name = "0")]
        [EnumMember(Value = "0")]
        [Subtitle("Entrada", "0", "Entrada")]
        Entrada = 0,

        [XmlEnum(Name = "1")]
        [EnumMember(Value = "1")]
        [Subtitle("Saida", "1", "Saída")]
        Saida = 1
    }
}
