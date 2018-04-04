using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFe.COFINS
{
    [XmlRoot(ElementName = "COFINSNT")]
    public class COFINSNT : COFINS
    {
        public COFINSNT() { }

        public COFINSNT(string codigoSituacaoTributaria) : base(codigoSituacaoTributaria) { }
    }
}
