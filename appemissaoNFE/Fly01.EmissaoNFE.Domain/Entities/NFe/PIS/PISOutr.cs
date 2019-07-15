using System.Xml.Serialization;
using Fly01.Core;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.EmissaoNFE.Domain.Enums;


namespace Fly01.EmissaoNFE.Domain.Entities.NFe.PIS
{
    [XmlRoot(ElementName = "PISOutr")]
    public class PISOutr : PIS
    {
        public PISOutr() { }

        public PISOutr(string codigoSituacaoTributaria, TipoCRT tipoCRT) : base(codigoSituacaoTributaria, tipoCRT) { }

        [XmlIgnore]
        public double? ValorBCDoPIS { get; set; }
        [XmlElement(ElementName = "vBC")]
        public string ValorBCDoPISString
        {
            get { return ValorBCDoPIS.HasValue ? ValorBCDoPIS.Value.ToString("0.00").Replace(",", ".") : "0.00"; }
            set { ValorBCDoPIS = double.Parse(value.Replace(".", ","), AppDefaults.CultureInfoDefault); }
        }
        public bool ShouldSerializeValorBCDoPISString()
        {
            return ValorBCDoPIS.HasValue;
        }

        [XmlIgnore]
        public double? PercentualPIS { get; set; }
        [XmlElement(ElementName = "pPIS")]
        public string PercentualPISString
        {
            get { return PercentualPIS.HasValue ? PercentualPIS.Value.ToString("0.00").Replace(",", ".") : "0.00"; }
            set { PercentualPIS = double.Parse(value.Replace(".", ","), AppDefaults.CultureInfoDefault); }
        }
        public bool ShouldSerializePercentualPISString()
        {
            return PercentualPIS.HasValue;
        }

        [XmlIgnore]
        public double? QuantidadeVendida { get; set; }
        [XmlElement(ElementName = "qBCProd")]
        public string QuantidadeVendidaString
        {
            get { return QuantidadeVendida.HasValue ? QuantidadeVendida.Value.ToString("0.00").Replace(",", ".") : "0.00"; }
            set { QuantidadeVendida = double.Parse(value.Replace(".", ","), AppDefaults.CultureInfoDefault); }
        }
        public bool ShouldSerializeQuantidadeVendidaString()
        {
            return QuantidadeVendida.HasValue;
        }

        [XmlIgnore]
        public double? AliquotaPISST { get; set; }
        [XmlElement(ElementName = "vAliqProd")]
        public string AliquotaPISSTString
        {
            get { return AliquotaPISST.HasValue ? AliquotaPISST.Value.ToString("0.00").Replace(",", ".") : "0.00"; }
            set { AliquotaPISST = double.Parse(value.Replace(".", ","), AppDefaults.CultureInfoDefault); }
        }
        public bool ShouldSerializeAliquotaPISSTString()
        {
            return AliquotaPISST.HasValue;
        }

        [XmlIgnore]
        public double? ValorPIS { get; set; }

        [XmlElement(ElementName = "vPIS", IsNullable = true)]
        public string ValorPISString
        {
            get { return ValorPIS.HasValue ? ValorPIS.Value.ToString("0.00").Replace(",", ".") : "0.00"; }
            set { ValorPIS = double.Parse(value.Replace(".", ","), AppDefaults.CultureInfoDefault); }
        }
        public bool ShouldSerializeValorPISString()
        {
            return ValorPIS.HasValue;
        }
    }
}
