using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFe
{
    [XmlRoot("autXML")]
    public class Autorizados
    {
        /// <summary>
        /// CNPJ de pessoa autorizada a baixar o XML da NFe
        /// </summary>
        [XmlElement(ElementName = "CNPJ", IsNullable = true)]
        public string CNPJ { get; set; }

        public bool ShouldSerializeCNPJ()
        {
            return !string.IsNullOrEmpty(CNPJ) && CNPJ.Length == 14;
        }

        /// <summary>
        /// CPF de pessoa autorizada a baixar o XML da NFe
        /// </summary>
        [XmlElement(ElementName = "CPF", IsNullable = true)]
        public string CPF { get; set; }

        public bool ShouldSerializeCPF()
        {
            return !string.IsNullOrEmpty(CPF) && CPF.Length == 11;
        }

    }
}