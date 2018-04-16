using Fly01.Core.Entities.Attribute;
using System.Xml.Serialization;

namespace Fly01.Core.Entities.Domains.Enum
{
    public enum TipoTributacaoICMS
    {
        [XmlEnum(Name = "101")]
        [Subtitle("TributadaComPermissaoDeCredito", "101 - Tributada pelo Simples Nacional com permissão de crédito")]
        TributadaComPermissaoDeCredito = 101,

        [XmlEnum(Name = "102")]
        [Subtitle("TributadaSemPermissaoDeCredito", "102 - Tributada pelo Simples Nacional sem permissão de crédito")]
        TributadaSemPermissaoDeCredito = 102,

        [XmlEnum(Name = "103")]
        [Subtitle("IsencaoParaFaixaDeReceitaBruta", "103 - Isenção do ICMS no Simples Nacional para faixa de receita bruta")]
        IsencaoParaFaixaDeReceitaBruta = 103,

        [XmlEnum(Name = "201")]
        [Subtitle("TributadaComPermissaoDeCreditoST", "201 - Tributada pelo Simples Nacional com permissão de crédito e com cobrança do ICMS por substituição tributária")]
        TributadaComPermissaoDeCreditoST = 201,

        [XmlEnum(Name = "202")]
        [Subtitle("TributadaSemPermissaoDeCreditoST", "202 - Tributada pelo Simples Nacional sem permissão de crédito e com cobrança do ICMS por substituição tributária")]
        TributadaSemPermissaoDeCreditoST = 202,

        [XmlEnum(Name = "203")]
        [Subtitle("IsencaoParaFaixaDeReceitaBrutaST", "203 - Isenção do ICMS no Simples Nacional para faixa de receita bruta e com cobrança do ICMS por substituição tributária")]
        IsencaoParaFaixaDeReceitaBrutaST = 203,

        [XmlEnum(Name = "300")]
        [Subtitle("Imune", "300 - Imune")]
        Imune = 300,

        [XmlEnum(Name = "400")]
        [Subtitle("NaoTributadaPeloSN", "400 - Não tributada pelo Simples Nacional")]
        NaoTributadaPeloSN = 400,

        [XmlEnum(Name = "500")]
        [Subtitle("ICMSSubstituidoOuAntecipado", "500 - ICMS cobrado anteriormente por substituição tributária (substituído) ou por antecipação")]
        ICMSSubstituidoOuAntecipado = 500,

        [XmlEnum(Name = "900")]
        [Subtitle("Outros", "900 - Outros")]
        Outros = 900
    }
}