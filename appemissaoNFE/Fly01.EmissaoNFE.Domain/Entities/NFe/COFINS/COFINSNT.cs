using System.Xml.Serialization;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.EmissaoNFE.Domain.Enums;

namespace Fly01.EmissaoNFE.Domain.Entities.NFe.COFINS
{
    [XmlRoot(ElementName = "COFINSNT")]
    public class COFINSNT : COFINS
    {
        public COFINSNT() { }

        public COFINSNT(string codigoSituacaoTributaria, TipoCRT tipoCRT) : base(codigoSituacaoTributaria, tipoCRT) { }
    }
}
