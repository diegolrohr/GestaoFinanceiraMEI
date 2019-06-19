using System.Xml.Serialization;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.EmissaoNFE.Domain.Enums;


namespace Fly01.EmissaoNFE.Domain.Entities.NFe.PIS
{
    [XmlRoot(ElementName = "PISNT")]
    public class PISNT : PIS
    {
        public PISNT() { }

        public PISNT(string codigoSituacaoTributaria, TipoCRT tipoCRT) : base(codigoSituacaoTributaria, tipoCRT) { }
    }
}
