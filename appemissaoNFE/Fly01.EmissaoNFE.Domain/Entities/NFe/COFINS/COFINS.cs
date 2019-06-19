using System.Xml.Serialization;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.EmissaoNFE.Domain.Enums;

namespace Fly01.EmissaoNFE.Domain.Entities.NFe.COFINS
{
    [XmlInclude(typeof(COFINSOutr))]
    public abstract class COFINS
    {
        public COFINS() { }

        public COFINS(string codigoSituacaoTributaria, TipoCRT tipoCRT)
        {
            CodigoSituacaoTributaria = codigoSituacaoTributaria;
            TipoCRT = tipoCRT;
        }

        [XmlElement(ElementName = "CST")]
        public string CodigoSituacaoTributaria { get; set; }

        [XmlIgnore]
        public TipoCRT TipoCRT { get; set; }




    }
}
