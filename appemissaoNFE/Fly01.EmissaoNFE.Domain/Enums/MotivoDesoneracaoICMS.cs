using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Enums
{
    public enum MotivoDesoneracaoICMS
    {
        [XmlEnum(Name = "1")]
        Taxi = 1,

        [XmlEnum(Name = "2")]
        DeficienteFisico = 2,

        [XmlEnum(Name = "3")]
        ProdutorAgropecuario = 3,

        [XmlEnum(Name = "4")]
        FloristaLocadora = 4,

        [XmlEnum(Name = "5")]
        DiplomaticoConsular = 5,

        [XmlEnum(Name = "6")]
        MotocicletaDaAmazoniaOcidental = 6,

        [XmlEnum(Name = "7")]
        SUFRAMA = 7,

        [XmlEnum(Name = "8")]
        VendaOrgaosPublicos = 8,

        [XmlEnum(Name = "9")]
        Outros = 9
    }
}
