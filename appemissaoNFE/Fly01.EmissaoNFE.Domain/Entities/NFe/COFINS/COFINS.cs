using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFe.COFINS
{
    [XmlInclude(typeof(COFINSOutr))]
    public abstract class COFINS
    {
        public COFINS() { }

        public COFINS(string codigoSituacaoTributaria)
        {
            CodigoSituacaoTributaria = codigoSituacaoTributaria;
        }

        [XmlElement(ElementName = "CST")]
        public string CodigoSituacaoTributaria { get; set; }
    }
}
