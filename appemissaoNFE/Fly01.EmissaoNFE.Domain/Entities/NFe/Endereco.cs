using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
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
        /// 
        [JsonProperty("xLgr")]
        [XmlElement(ElementName = "xLgr")]
        public string Logradouro { get; set; }

        [Required]
        [MaxLength(60)]
        /// <summary>
        /// informar o número do endereço do destinatário, campo obrigatório. Informar S/N ou . (ponto) ou - (traço) para evitar falha de schema XML quando não houver número.
        /// </summary>
        /// 
        [JsonProperty("nro")]
        [XmlElement(ElementName = "nro")]
        public string Numero { get; set; }

        [MaxLength(60)]
        [JsonProperty("xCpl")]
        [XmlElement(ElementName = "xCpl")]
        public string Complemento { get; set; }

        [Required]
        [MaxLength(60)]
        [JsonProperty("xBairro")]
        [XmlElement(ElementName = "xBairro")]
        public string Bairro { get; set; }

        [Required]
        [StringLength(7, ErrorMessage = "Código IBGE inválido.")]
        /// <summary>
        /// informar o código do município na codificação do IBGE com 7 dígitos
        /// </summary>
        /// 
        [JsonProperty("cMun")]
        [XmlElement(ElementName = "cMun")]
        public string CodigoMunicipio { get; set; }

        [Required]
        [MaxLength(60)]
        /// <summary>
        /// informar o nome do município
        /// </summary>
        /// 
        [JsonProperty("xMun")]
        [XmlElement(ElementName = "xMun")]
        public string Municipio { get; set; }

        /// <summary>
        /// informar a sigla da UF
        /// </summary>
        /// 
        [Required]
        [StringLength(2, ErrorMessage = "UF inválida.")]
        [JsonProperty("UF")]
        [XmlElement(ElementName = "UF")]
        public string UF { get; set; }

        [StringLength(8, ErrorMessage = "CEP inválido.")]
        /// <summary>
        /// informar o CEP, sem formatação ou máscara, pode ser omitido
        /// </summary>
        /// 
        [JsonProperty("CEP")]
        [XmlElement(ElementName = "CEP")]
        public string Cep { get; set; }

        /// <summary>
        /// Informe o código bacen do país, de acordo com o banco central do Brasil
        /// </summary>
        [XmlElement(ElementName = "cPais")]
        public string PaisCodigoBacen { get; set; }

        public bool ShouldSerializePaisCodigoBacen()
        {
            return !string.IsNullOrEmpty(PaisNome) && !string.IsNullOrEmpty(PaisCodigoBacen);
        }

        /// <summary>
        /// Informa o nome do país
        /// </summary>
        [XmlElement(ElementName = "xPais")]
        public string PaisNome { get; set; }

        public bool ShouldSerializePaisNome()
        {
            return !string.IsNullOrEmpty(PaisNome) && !string.IsNullOrEmpty(PaisCodigoBacen);
        }

        [MaxLength(14)]
        /// <summary>
        /// informar o telefone com DDD + número, sem formatação
        /// </summary>
        [JsonProperty("fone")]
        [XmlElement(ElementName = "fone", IsNullable = true)]
        public string Fone { get; set; }

        public bool ShouldSerializeFone()
        {
            return !string.IsNullOrEmpty(Fone);
        }
    }
}
