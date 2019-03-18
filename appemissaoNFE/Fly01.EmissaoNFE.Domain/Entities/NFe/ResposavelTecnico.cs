using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFe
{
    public class ResposavelTecnico
    {
        /// <summary>
        /// informar o CNPJ da pessoa jurídica resposável técnica pelo sistema utilizado na emissão do documento fiscal eletrônico.
        /// </summary>
        [XmlAttribute("CNPJ")]
        public string CNPJ { get; set; }

        /// <summary>
        /// informar o nome da pessoa a ser contatada.
        /// </summary>
        [XmlAttribute("xContato")]
        public string Contato { get; set; }

        /// <summary>
        /// informar o e-mail da pessoa jurídica a ser contatada.
        /// </summary>
        [XmlAttribute("email")]
        public string Email { get; set; }

        /// <summary>
        /// informar o telefone da pessoa jurídica a ser contatada.
        /// </summary>
        [XmlAttribute("fone")]
        public string Fone { get; set; }

        /// <summary>
        /// informar o identificador do código de segurança do responsável técnico
        /// </summary>
        [XmlAttribute("idCSRT")]
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
        [XmlAttribute("hashCSRT")]
        public string HashCSRT { get; set; }

        public bool ShouldSerializeHashCSRT()
        {
            return (!string.IsNullOrEmpty(IdentificadorCodigoResponsavelTecnico) && !string.IsNullOrEmpty(CodigoResponsavelTecnico) && !string.IsNullOrEmpty(HashCSRT));
        }
    }
}
