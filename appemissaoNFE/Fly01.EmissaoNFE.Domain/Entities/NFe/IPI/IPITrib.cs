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
        [XmlIgnore]
        public double? ValorBaseCalculo { get; set; }

        [XmlElement(ElementName = "vBC")]
        public string ValorBaseCalculoString
        {
            get { return ValorBaseCalculo.Value.ToString("0.00").Replace(",", "."); }
            set { ValorBaseCalculo = double.Parse(value.Replace(".", ",")); }
        }

        public bool ShouldSerializeValorBaseCalculoString()
        {
            return !string.IsNullOrEmpty(ValorBaseCalculoString);
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
            set { ValorIPI = double.Parse(value.Replace(".", ",")); }
        }

        /// <summary>
        /// Deve ser informado quando preenchido o Grupo Tributos Devolvidos na emissão de nota finNFe=4 (devolução) nas operações com não contribuintes do IPI.
        /// </summary>
        [XmlIgnore]
        public double? ValorIPIDevolucao { get; set; }

        [XmlElement(ElementName = "vIPIDevol")]
        public string ValorIPIDevolucaoString
        {
            get { return ValorIPIDevolucao.HasValue ? ValorIPIDevolucao.Value.ToString("0.00").Replace(",", ".") : "0.00"; }
            set { ValorIPIDevolucao = double.Parse(value.Replace(".", ",")); }
        }

        public bool ShouldSerializeValorIPIDevolucaoString()
        {
            return ValorIPIDevolucao.HasValue && ValorIPIDevolucao.Value > 0;
        }
    }
}