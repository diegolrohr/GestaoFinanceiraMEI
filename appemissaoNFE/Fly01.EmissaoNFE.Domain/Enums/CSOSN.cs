using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Enums
{
    /// <summary>
    /// Tabela de CSOSN: Código de Situação da Operação - SIMPLES NACIONAL
    /// </summary>
    public enum CSOSN
    {
        [XmlEnum(Name = "101")]
        TributadaComPermissaoDeCredito = 101,

        [XmlEnum(Name = "102")]
        TributadaSemPermissaoDeCredito = 102,

        [XmlEnum(Name = "103")]
        IsencaoParaFaixaDeReceitaBruta = 103,

        [XmlEnum(Name = "201")]
        TributadaComPermissaoDeCreditoST = 201,

        [XmlEnum(Name = "202")]
        TributadaSemPermissaoDeCreditoST = 202,

        [XmlEnum(Name = "203")]
        IsencaoParaFaixaDeReceitaBrutaST = 203,

        [XmlEnum(Name = "300")]
        Imune = 300,

        [XmlEnum(Name = "400")]
        NaoTributadaPeloSN = 400,

        [XmlEnum(Name = "500")]
        ICMSSubstituidoOuAntecipado = 500,

        [XmlEnum(Name = "900")]
        Outros = 900
    }
}
