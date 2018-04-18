using System.Xml.Serialization;
namespace Fly01.EmissaoNFE.Domain.Entities.NFe
{
    public class PISST
    {
        /// <summary>
        /// Informar o Valor da BC do PIS ST, este campo deve ser informado em caso de alíquota ad valorem.
        /// </summary>
        [XmlElement(ElementName = "vBC", IsNullable = true)]
        public double? ValorBC { get; set; }

        public bool ShouldSerializeValorBC()
        {
            return ValorBC.HasValue;
        }

        /// <summary>
        /// Informar a alíquota percentual do PIS ST, este campo deve ser informado em caso de alíquota ad valorem.
        /// </summary>
        [XmlElement(ElementName = "pPIS", IsNullable = true)]
        public double? AliquotaPercentual { get; set; }

        public bool ShouldSerializeAliquotaPercentual()
        {
            return AliquotaPercentual.HasValue;
        }

        /// <summary>
        /// Informar o Valor do PIS ST
        /// </summary>
        [XmlElement(ElementName = "vPIS")]
        public double ValorPISST { get; set; }

        /// <summary>
        /// Informar a quantidade vendida, este campo deve ser informado em caso de alíquota específica.
        /// </summary>
        [XmlElement(ElementName = "qBCProd", IsNullable = true)]
        public double? QuantidadeVendida { get; set; }

        public bool ShouldSerializeQuantidadeVendida()
        {
            return QuantidadeVendida.HasValue;
        }

        /// <summary>
        /// Informar a alíquota do PIS ST em reais, este campo deve ser informado em caso de alíquota específica.
        /// </summary>
        [XmlElement(ElementName = "vAliqProd", IsNullable = true)]
        public double? AliquotaPISST { get; set; }

        public bool ShouldSerializeAliquotaPISST()
        {
            return AliquotaPISST.HasValue;
        }
    }
}
