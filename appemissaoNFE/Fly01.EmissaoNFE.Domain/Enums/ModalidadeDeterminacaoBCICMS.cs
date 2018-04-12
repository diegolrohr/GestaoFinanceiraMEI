using Fly01.Core.Entities.Attribute;
using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Enums
{
    public enum ModalidadeDeterminacaoBCICMS
    {
        [XmlEnum(Name = "0")]
        [Subtitle("MargemValorAgregado", "0", "Margem Valor Agregado (%)")]
        MargemValorAgregado = 0,

        [XmlEnum(Name = "1")]
        [Subtitle("Pauta", "1", "Pauta")]
        Pauta = 1,

        [XmlEnum(Name = "2")]
        [Subtitle("PrecoTabeladoMax", "2", "Preço Tabelado Máx. (Valor)")]
        PrecoTabeladoMax = 2,

        [XmlEnum(Name = "3")]
        [Subtitle("ValorDaOperacao", "3", "Valor da Operação")]
        ValorDaOperacao = 3  
    }
}
