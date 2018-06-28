using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFe
{
    public class NFReferenciada
    {
        /// <summary>
        /// Informar a chave de acesso da NF-e referenciada
        /// </summary>
        [XmlElement(ElementName = "refNFe")]
        public string ChaveNFeReferenciada { get; set; }

        public bool ShouldSerializeChaveNFeReferenciada()
        {
            return !string.IsNullOrEmpty(ChaveNFeReferenciada);
        }
    }
}
