using System.Xml.Serialization;
using Fly01.Core.Helpers.Attribute;

namespace Fly01.Core.Entities.Domains.Enum
{
    /// <summary>
    /// Tabela de CRT:Código de Regime Tributário
    /// </summary>
    public enum TipoCRT
    {
        [XmlEnum(Name = "1")]
        [Subtitle("SimplesNacional", "Simples Nacional", "SimplesNacional")]
        SimplesNacional = 1,

        [XmlEnum(Name = "2")]
        [Subtitle("ExcessoSublimiteDeReceitaBruta", "Simples Nacional - Excesso de Sublimite de Receita Bruta", "Simples Nacional - Excesso de Sublimite de Receita Bruta")]
        ExcessoSublimiteDeReceitaBruta = 2,

        [XmlEnum(Name = "3")]
        [Subtitle("RegimeNormal", "Regime Normal", "RegimeNormal")]
        RegimeNormal = 3
    }
}
