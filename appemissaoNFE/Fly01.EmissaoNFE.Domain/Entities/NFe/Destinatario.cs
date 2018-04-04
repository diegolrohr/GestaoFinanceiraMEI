using Fly01.EmissaoNFE.Domain.Enums;
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
        [XmlElement(ElementName = "CNPJ")]
        public string Cnpj { get; set; }

        [Required]
        [StringLength(11, ErrorMessage = "CPF inválido.")]
        /// <summary>
        /// CPF do destinatário, sem formatação ou máscara
        /// </summary>
        [XmlElement(ElementName = "CPF")]
        public string Cpf { get; set; }

        [MaxLength(60)]
        /// <summary>
        /// informar a razão social do destinatário, pode ser omitida no caso de NFC-e.
        /// </summary>
        [XmlElement(ElementName = "xNome")]
        public string Nome { get; set; }
        
        [Required]
        /// <summary>
        /// Informar o grupo de Endereço
        /// </summary>
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
        [XmlElement(ElementName = "indIEDest")]
        public IndInscricaoEstadual IndInscricaoEstadual { get; set; }

        [MaxLength(14)]
        /// <summary>
        /// informar a IE do destinatário (somente quando informar a tag indIEDest=1), sem formatação ou máscara
        /// A tag não aceita mais a literal "ISENTO", assim só informe a Inscrição Estadual, isto é só informe está tag quando informar a tag indIEDest = 1.
        /// Nota: Não informar esta tag no caso da NFC-e.
        /// </summary>
        [XmlElement(ElementName = "IE")]
        public string InscricaoEstadual { get; set; }

        public bool ShouldSerializeInscricaoEstadual()
        {
            return IndInscricaoEstadual == IndInscricaoEstadual.ContribuinteICMS;
        }
    }
}
