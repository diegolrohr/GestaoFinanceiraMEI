using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFe
{
    public class Endereco
    {
        [Required]
        [MaxLength(60)]
        /// <summary>
        /// informar o logradouro do destinatário, o grupo de informações do endereço do destinatário pode ser omitido no caso de NFC-e.
        /// </summary>
        [XmlElement(ElementName = "xLgr")]
        public string Logradouro { get; set; }

        [Required]
        [MaxLength(60)]
        /// <summary>
        /// informar o número do endereço do destinatário, campo obrigatório. Informar S/N ou . (ponto) ou - (traço) para evitar falha de schema XML quando não houver número.
        /// </summary>
        [XmlElement(ElementName = "nro")]
        public string Numero { get; set; }

        [MaxLength(60)]
        [XmlElement(ElementName = "xCpl")]
        public string Complemento { get; set; }

        [Required]
        [MaxLength(60)]
        [XmlElement(ElementName = "xBairro")]
        public string Bairro { get; set; }

        [Required]
        [StringLength(7, ErrorMessage = "Código IBGE inválido.")]
        /// <summary>
        /// informar o código do município na codificação do IBGE com 7 dígitos
        /// </summary>
        [XmlElement(ElementName = "cMun")]
        public string CodigoMunicipio { get; set; }

        [Required]
        [MaxLength(60)]
        /// <summary>
        /// informar o nome do município
        /// </summary>
        [XmlElement(ElementName = "xMun")]
        public string Municipio { get; set; }

        [Required]
        [StringLength(2, ErrorMessage = "UF inválida.")]
        /// <summary>
        /// informar a sigla da UF

        /// </summary>
        [XmlElement(ElementName = "UF")]
        public string UF { get; set; }

        [StringLength(8, ErrorMessage = "CEP inválido.")]
        /// <summary>
        /// informar o CEP, sem formatação ou máscara, pode ser omitido
        /// </summary>
        [XmlElement(ElementName = "CEP")]
        public string Cep { get; set; }

        [MaxLength(14)]
        /// <summary>
        /// informar o telefone com DDD + número, sem formatação
        /// </summary>
        [XmlElement(ElementName = "fone", IsNullable = true)]
        public string Fone { get; set; }

        public bool ShouldSerializeFone()
        {
            return !string.IsNullOrEmpty(Fone);
        }
    }
}
