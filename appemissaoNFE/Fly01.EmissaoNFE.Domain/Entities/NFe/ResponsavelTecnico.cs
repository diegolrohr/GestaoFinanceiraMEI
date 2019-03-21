using Fly01.Core.Entities.Domains;
using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFe
{
    public class ResponsavelTecnico
    {
        /// <summary>
        /// informar o CNPJ da pessoa jurídica resposável técnica pelo sistema utilizado na emissão do documento fiscal eletrônico.
        /// </summary>
        [XmlElement("CNPJ")]
        public string CNPJ { get; set; }

        /// <summary>
        /// informar o nome da pessoa a ser contatada.
        /// </summary>
        [XmlElement("xContato")]
        public string Contato { get; set; }

        /// <summary>
        /// informar o e-mail da pessoa jurídica a ser contatada.
        /// </summary>
        [XmlElement("email")]
        public string Email { get; set; }

        /// <summary>
        /// informar o telefone da pessoa jurídica a ser contatada.
        /// </summary>
        [XmlElement("fone")]
        public string Fone { get; set; }

        /// <summary>
        /// informar o identificador do código de segurança do responsável técnico
        /// </summary>
        [XmlElement("idCSRT")]
        public string IdentificadorCodigoResponsavelTecnico { get; set; }

        public bool ShouldSerializeIdentificadorCodigoResponsavelTecnico()
        {
            return (!string.IsNullOrEmpty(IdentificadorCodigoResponsavelTecnico) && !string.IsNullOrEmpty(CodigoResponsavelTecnico) && !string.IsNullOrEmpty(HashCSRT));
        }

        /// <summary>
        /// Informar o CSRT - código de segurança do responsável técnico
        /// </summary>       
        [XmlIgnore]
        public string CodigoResponsavelTecnico { get; set; }

        /// <summary>
        /// Geração do hashCSRT
        /// </summary>
        [XmlElement("hashCSRT")]
        public string HashCSRT { get; set; }

        public bool ShouldSerializeHashCSRT()
        {
            return (!string.IsNullOrEmpty(IdentificadorCodigoResponsavelTecnico) && !string.IsNullOrEmpty(CodigoResponsavelTecnico) && !string.IsNullOrEmpty(HashCSRT));
        }
    }
}
