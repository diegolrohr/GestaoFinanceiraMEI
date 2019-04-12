using Fly01.Core.Entities.Domains.Enum;
using Fly01.EmissaoNFE.Domain.Enums;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFe
{
    public class Destinatario
    {
    	[Required]
        [StringLength(14, ErrorMessage = "CNPJ inválido.")]
        /// <summary>
        /// informar o CNPJ do destinatário, sem formatação ou máscara
        /// </summary>
        /// 
        [JsonProperty("CNPJ")]
        [XmlElement(ElementName = "CNPJ")]
        public string Cnpj { get; set; }

        [Required]
        [StringLength(11, ErrorMessage = "CPF inválido.")]
        /// <summary>
        /// CPF do destinatário, sem formatação ou máscara
        /// </summary>
        [JsonProperty("CPF")]
        [XmlElement(ElementName = "CPF")]
        public string Cpf { get; set; }

        /// <summary>
        /// No caso de operação com o exterior, ou para comprador estrangeiro informar com o número do passaporte ou outro documento legal para identificar pessoa estrangeira.
        /// Campo aceita valor Nulo.
        /// </summary>
        [JsonProperty("idEstrangeiro")]
        [StringLength(20)]
        [XmlElement(ElementName = "idEstrangeiro")]
        public string IdentificacaoEstrangeiro { get; set; }

        public bool ShouldSerializeIdentificacaoEstrangeiro()
        {
            return EhExportacao();
        }

        [MaxLength(60)]
        /// <summary>
        /// informar a razão social do destinatário, pode ser omitida no caso de NFC-e.
        /// </summary>
        [JsonProperty("xNome")]
        [XmlElement(ElementName = "xNome")]
        public string Nome { get; set; }
        
        [Required]
        /// <summary>
        /// Informar o grupo de Endereço
        /// </summary>
        [JsonProperty("enderDest")]
        [XmlElement(ElementName = "enderDest")]
        public Endereco Endereco { get; set; }

        [Required]
        /// <summary>
        /// Indicador da IE do Destinatário, informar:
        /// 1 - Contribuinte ICMS (informar a tag IE do destinatário);
        /// 2 - Contribuinte isento de Inscrição no cadastro de Contribuintes do ICMS - não informar a tag IE;
        /// 9 - Não Contribuinte, que pode ou não possuir Inscrição Estadual no Cadastro de Contribuintes do ICMS - não informar a tag IE.
        /// Nota 1: No caso de NFC-e informar indIEDest=9 e não informar a tag IE do destinatário;
        /// Nota 2: No caso de operação com o Exterior informar indIEDest=9 e não informar a tag IE do destinatário;
        /// Nota 3: No caso de Contribuinte Isento de Inscrição(indIEDest= 2), não informar a tag IE do destinatário.
        /// </summary>
        /// 
        [JsonProperty("indIEDest")]
        [XmlElement(ElementName = "indIEDest")]
        public TipoIndicacaoInscricaoEstadual IndInscricaoEstadual { get; set; }

        [MaxLength(14)]
        /// <summary>
        /// informar a IE do destinatário (somente quando informar a tag indIEDest=1), sem formatação ou máscara
        /// A tag não aceita mais a literal "ISENTO", assim só informe a Inscrição Estadual, isto é só informe está tag quando informar a tag indIEDest = 1.
        /// Quando o for emitida uma NF-e para Destinatário, identificado como Isento (indIEDest = 2) ou Não Contribuinte (indIEDest = 9),
        /// que possui Inscrição Estadual (IE) ativa no seu Estado (UF) e essa não for informada em seus Dados, 
        /// Quando for exportação a ie deve sair em branco
        /// </summary>        
        [XmlIgnore]
        public string InscricaoEstadual { get; set; }

        [JsonProperty("IE")]
        [XmlElement(ElementName = "IE")]
        public string IE
        {
            get { return EhExportacao() ? " " : InscricaoEstadual; }
            set { InscricaoEstadual = value; }
        }

        public bool ShouldSerializeIE()
        {
            return !string.IsNullOrEmpty(InscricaoEstadual) || EhExportacao();
        }

        public bool EhExportacao()
        {
            return Endereco?.UF == "EX";
        }
    }
}
