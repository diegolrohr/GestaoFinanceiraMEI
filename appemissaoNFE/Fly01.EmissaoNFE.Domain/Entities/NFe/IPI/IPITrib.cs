using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFe.IPI
{
    [XmlRoot("IPITrib")]
    public class IPITrib : IPI
    {
        /// <summary>
        /// Informar a quantidade total na unidade padrão de tributação, este campo deve ser informado em caso de alíquota específica.
        /// </summary>
        [XmlElement(ElementName = "qUnid")]
        public double? QtdTotalUnidadeTributavel { get; set; }

        public bool ShouldSerializeQtdTotalUnidadeTributavel()
        {
            return QtdTotalUnidadeTributavel.HasValue;
        }

        /// <summary>
        /// Informar o Valor por Unidade Tributável, este campo deve ser informado em caso de alíquota específica.
        /// </summary>
        [XmlElement(ElementName = "vUnid")]
        public double? ValorUnidadeTributavel { get; set; }

        public bool ShouldSerializeValorUnidadeTributavel()
        {
            return ValorUnidadeTributavel.HasValue;
        }

        /// <summary>
        /// Informar o Valor da BC do IPI, este campo deve ser informado em caso de alíquota ad valorem.
        /// </summary>
        [XmlElement(ElementName = "vBC")]
        public double? ValorBaseCalculo { get; set; }

        public bool ShouldSerializeValorBaseCalculo()
        {
            return ValorBaseCalculo.HasValue;
        }

        /// <summary>
        /// Informar a alíquota percentual do IPI, este campo deve ser informado em caso de alíquota ad valorem. 
        /// </summary>
        [XmlElement(ElementName = "pIPI")]
        public double? PercentualIPI { get; set; }

        public bool ShouldSerializePercentualIPI()
        {
            return PercentualIPI.HasValue;
        }

        /// <summary>
        /// Informar o Valor do IPI
        /// </summary>
        [XmlIgnore]
        public double ValorIPI { get; set; }

        [XmlElement(ElementName = "vIPI")]
        public string ValorIPIString
        {
            get { return ValorIPI.ToString("0.00").Replace(",", "."); }
            set { ValorIPI = double.Parse(value); }
        }
    }
}