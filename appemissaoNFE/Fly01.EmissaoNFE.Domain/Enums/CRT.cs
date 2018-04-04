using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Enums
{
    /// <summary>
    /// Tabela de CRT:Código de Regime Tributário
    /// </summary>
    public enum CRT
    {
        [XmlEnum(Name = "1")]
        SimplesNacional = 1,

        [XmlEnum(Name = "2")]
        ExcessoSublimiteDeReceitaBruta = 2,

        [XmlEnum(Name = "3")]
        RegimeNormal = 3
    }
}
