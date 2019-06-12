using Fly01.Core.Helpers.Attribute;
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
        Outros = 900,
        //lucro Presumido
        [XmlEnum(Name = "00")]
        [Subtitle("TributadaIntegralmente", "00 - Tributada Integralmente")]
        TributadaIntegralmente = 0,

        [XmlEnum(Name = "10")]
        [Subtitle("TributadaComCobrancaDeSubstituicao", "10 - Tributada Com cobrança de substituição Tributária")]
        TributadaComCobrancaDeSubstituicao = 10,

        [XmlEnum(Name = "20")]
        [Subtitle("ComReducaoDeBaseDeCalculo", "20 - Com Redução de Base de Calculo")]
        ComReducaoDeBaseDeCalculo = 20,

        [XmlEnum(Name = "30")]
        [Subtitle("IsentaOuNaoTributadaPorST", "30 - Isenta ou Nao Tributada Por Substituição Tributária")]
        IsentaOuNaoTributadaPorST = 30,

        [XmlEnum(Name = "40")]
        [Subtitle("Isenta", "40 - Isenta")]
        Isenta = 40,

        [XmlEnum(Name = "41")]
        [Subtitle("NaoTributada", "41 - Não Tributada")]
        NaoTributada = 41,

        [XmlEnum(Name = "50")]
        [Subtitle("ComSuspensao", "50 - Com Suspensão")]
        ComSuspensao = 50,

        [XmlEnum(Name = "51")]
        [Subtitle("Diferimento", "51 - Diferimento")]
        Diferimento = 51,

        [XmlEnum(Name = "60")]
        [Subtitle("ICMSCobradoAnteriormentePorST", "60 - ICMS Cobrado Anteriormente Por Substituição Tributária")]
        ICMSCobradoAnteriormentePorST = 60,

        [XmlEnum(Name = "70")]
        [Subtitle("ComRedDeBaseDeST", "70 - Com Redução de calculo e cobrado por Substituição Tributária")]
        ComRedDeBaseDeST = 70,

        [XmlEnum(Name = "90")]
        [Subtitle("Outros90", "90 - Outros")]
        Outros90 = 90
    }
}