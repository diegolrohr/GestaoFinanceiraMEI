using Fly01.Core.Entities.Attribute;
using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Enums
{
    /// <summary>
    /// Tabela de CST do IPI
    /// </summary>
    public enum CSTIPI
    {
        [XmlEnum(Name = "00")]
        [Subtitle("EntradaComRecuperacaoDeCredito", "00", "Entrada com Recuperação de Crédito")]
        EntradaComRecuperacaoDeCredito = 0,

        [XmlEnum(Name = "01")]
        [Subtitle("EntradaTributavelComAliquotaZero", "01", "Entrada Tributada com Alíquota Zero")]
        EntradaTributavelComAliquotaZero = 1,

        [XmlEnum(Name = "02")]
        [Subtitle("EntradaIsenta", "02", "Entrada Isenta")]
        EntradaIsenta = 2,

        [XmlEnum(Name = "03")]
        [Subtitle("EntradaNaoTributada", "03", "Entrada Não Tributada")]
        EntradaNaoTributada = 3,

        [XmlEnum(Name = "04")]
        [Subtitle("EntradaImune", "04", "Entrada Imune")]
        EntradaImune = 4,

        [XmlEnum(Name = "05")]
        [Subtitle("EntradaComSuspensao", "05", "Entrada com Suspensão")]
        EntradaComSuspensao = 5,

        [XmlEnum(Name = "49")]
        [Subtitle("OutrasEntradas", "49", "Outras Entradas")]
        OutrasEntradas = 49,

        [XmlEnum(Name = "50")]
        [Subtitle("SaidaTributada", "50", "Saída Tributada")]
        SaidaTributada = 50,

        [XmlEnum(Name = "51")]
        [Subtitle("SaidaTributadaComAliquotaZero", "51", "Saída Tributável com Alíquota Zero")]
        SaidaTributadaComAliquotaZero = 51,

        [XmlEnum(Name = "52")]
        [Subtitle("SaidaIsenta", "52", "Saída Isenta")]
        SaidaIsenta = 52,

        [XmlEnum(Name = "53")]
        [Subtitle("SaidaNaoTributada", "53", "Saída Não Tributada")]
        SaidaNaoTributada = 53,

        [XmlEnum(Name = "54")]
        [Subtitle("SaidaImune", "54", "Saída Imune")]
        SaidaImune = 54,

        [XmlEnum(Name = "55")]
        [Subtitle("SaidaComSuspensao", "55", "Saída com Suspensão")]
        SaidaComSuspensao = 55,

        [XmlEnum(Name = "99")]
        [Subtitle("OutrasSaidas", "99", "Outras Saídas")]
        OutrasSaidas = 99

    }
}
