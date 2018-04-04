using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFe.PIS
{
    public abstract class PIS
    {
        public PIS()
        {

        }

        public PIS(string codigoSituacaoTributaria)
        {
            CodigoSituacaoTributaria = codigoSituacaoTributaria;
        }

        [XmlElement(ElementName = "CST")]
        public string CodigoSituacaoTributaria { get; set; }
    }
}
