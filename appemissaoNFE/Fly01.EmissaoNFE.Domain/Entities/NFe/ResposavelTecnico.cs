using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFe
{
    [XmlRoot("infRespTec")]
    public class ResposavelTecnico
    {
        /// <summary>
        /// informar o CNPJ da pessoa jurídica resposável técnica pelo sistema utilizado na emissão do documento fiscal eletrônico.
        /// </summary>
        /// 
        [XmlAttribute("CNPJ")]
        public string CNPJ { get; set; }

        /// <summary>
        /// informar o nome da pessoa a ser contatada.
        /// </summary>
        /// 
        [XmlAttribute("xContato")]
        public string Contato { get; set; }

        /// <summary>
        /// informar o e-mail da pessoa jurídica a ser contatada.
        /// </summary>
        /// 
        [XmlAttribute("email")]
        public string email { get; set; }

        /// <summary>
        /// informar o telefone da pessoa jurídica a ser contatada.
        /// </summary>
        /// 
        [XmlAttribute("fone")]
        public string fone { get; set; }

        /// <summary>
        /// informar o identificador do código de segurança do responsável técnico
        /// </summary>
        /// 
        [XmlAttribute("idCSRT_Opc")]
        public string IdentificadorCodigoResponsavelTecnico { get; set; }

        /// <summary>
        /// informar o CSRT - código de segurança do responsável técnico
        /// </summary>
        /// 
        [XmlAttribute("CSRT_Opc")]
        public string CodigoResponsavelTecnico { get; set; }


        /// <summary>
        /// informar a chave de acesso da NF-e
        /// </summary>
        /// 
        [XmlAttribute("chaveNFe_Opc")]
        public string ChaveAcessoNFE { get; set; }
    }
}
