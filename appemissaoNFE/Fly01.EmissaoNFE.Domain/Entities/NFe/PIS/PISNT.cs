using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFe.PIS
{
    [XmlRoot(ElementName = "PISNT")]
    public class PISNT : PIS
    {
        public PISNT() { }

        public PISNT(string codigoSituacaoTributaria) : base(codigoSituacaoTributaria) { }
    }
}
