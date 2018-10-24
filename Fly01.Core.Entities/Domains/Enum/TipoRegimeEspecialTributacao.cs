using Fly01.Core.Helpers.Attribute;
using System.Xml.Serialization;

namespace Fly01.Core.Entities.Domains.Enum
{
    public enum TipoRegimeEspecialTributacao
    {
        [XmlEnum(Name = "0")]
        [Subtitle("TributacaoNormal", "Tributação Normal")]
        TributacaoNormal = 0,

        [XmlEnum(Name = "1")]
        [Subtitle("MicroEmpresaMunicipal", "Micro Empresa Municipal (ME)")]
        MicroEmpresaMunicipal = 1,

        [XmlEnum(Name = "2")]
        [Subtitle("Estimativa", "Estimativa")]
        Estimativa = 2,

        [XmlEnum(Name = "3")]
        [Subtitle("SociedadeProfissionais", "Sociedade de Profissionais")]
        SociedadeProfissionais = 3,

        [XmlEnum(Name = "4")]
        [Subtitle("Cooperativa", "Cooperativa")]
        Cooperativa = 4,

        [XmlEnum(Name = "5")]
        [Subtitle("MEI", "Microempresário Individual (MEI)")]
        MEI = 5,

        [XmlEnum(Name = "6")]
        [Subtitle("MEEPP", "Microempresário e Empesa de Pequeno Porte (ME EPP)")]
        MEEPP = 6,

        [XmlEnum(Name = "7")]
        [Subtitle("MovimentoMensal", "Movimento Mensal /ISS /Fixo Autônomo")]
        MovimentoMensal = 7,

        //[XmlEnum(Name = "8")]
        //[Subtitle("SociedadeLimitada", "Sociedade Limitada /Média Empresa")]
        //SociedadeLimitada = 8,

        //[XmlEnum(Name = "9")]
        //[Subtitle("SociedadeAnonima", "Sociedade Anônima /Grande Empresa")]
        //SociedadeAnonima = 9,

        [XmlEnum(Name = "10")]
        [Subtitle("EIRELI", "Empresa Individual de Responsabilidade Limitada (EIRELI)")]
        EIRELI = 10,

        [XmlEnum(Name = "11")]
        [Subtitle("EmpresaIndividual", "Empresa Individual")]
        EmpresaIndividual = 11,

        [XmlEnum(Name = "12")]
        [Subtitle("EPP", "Empresa de Pequeno Porte (EPP)")]
        EPP = 12,

        [XmlEnum(Name = "13")]
        [Subtitle("Microempresario", "Microempresário")]
        Microempresario = 13,

        [XmlEnum(Name = "14")]
        [Subtitle("Outros", "Outros /Sem Vínculos")]
        Outros = 14,

        [XmlEnum(Name = "50")]
        [Subtitle("Nenhum", "Nenhum")]
        Nenhum = 50,

        [XmlEnum(Name = "51")]
        [Subtitle("NotaAvulsa", "NotaAvulsa")]
        NotaAvulsa = 51,
    }
}