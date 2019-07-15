using Fly01.Core;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.EmissaoNFE.Domain.Enums;
using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFe.ICMS
{
    [XmlRoot("ICMS20")]
    public class ICMS20 : ICMS 
    {
        public ICMS20()
        {

        }
        public ICMS20(OrigemMercadoria origemMercadoria, TipoTributacaoICMS codigoSituacaoOperacao,TipoCRT tipoCRT) : base(origemMercadoria,codigoSituacaoOperacao, tipoCRT)
        {
        }      

        [XmlElement(ElementName = "modBC")]
        public ModalidadeDeterminacaoBCICMS ModalidadeBC { get; set; }

        [XmlIgnore]
        public double PercentualReducaoBC { get; set; }

        [XmlElement("pRedBC")]
        public string PercentualReducaoBCString
        {
            get
            {
                return PercentualReducaoBC.ToString("0.0000").Replace(",", ".");
            }
            set { PercentualReducaoBC = double.Parse(value.Replace(".", ","), AppDefaults.CultureInfoDefault); }
        }

        [XmlIgnore]
        public double ValorBC { get; set; }

        [XmlElement("vBC")]
        public string ValorBCString
        {
            get
            {
                return ValorBC.ToString("0.00").Replace(",", ".");
            }
            set { ValorBC = double.Parse(value.Replace(".", ","), AppDefaults.CultureInfoDefault); }
        }

        [XmlIgnore]
        public double AliquotaICMS { get; set; }

        [XmlElement("pICMS")]
        public string AliquotaICMSString
        {
            get
            {
                return AliquotaICMS.ToString("0.0000").Replace(",", ".");
            }
            set { AliquotaICMS = double.Parse(value.Replace(".", ","), AppDefaults.CultureInfoDefault); }
        }

        [XmlIgnore]
        public double ValorICMS { get; set; }

        [XmlElement("vICMS")]
        public string ValorICMSString
        {
            get
            {
                return ValorICMS.ToString("0.00").Replace(",", ".");
            }
            set { ValorICMS = double.Parse(value.Replace(".", ","), AppDefaults.CultureInfoDefault); }
        }

        [XmlElement(ElementName = "motDesICMS", IsNullable = true)]
        public int? MotivoDesoneracaoICMS { get; set; }

        public bool ShouldSerializeMotivoDesoneracaoICMS()
        {
            return MotivoDesoneracaoICMS.HasValue;
        }

        [XmlIgnore]
        public double ValorBCFCP { get; set; }

        [XmlElement("vBCFCP")]
        public string ValorBCFCPString
        {
            get
            {
                return ValorBCFCP.ToString("0.00").Replace(",", ".");
            }
            set { ValorBCFCP = double.Parse(value.Replace(".", ","), AppDefaults.CultureInfoDefault); }
        }
        public bool ShouldSerializeValorBCFCPString()
        {
            return (ValorBCFCP > 0.0) && (AliquotaFCP > 0.0) && (ValorFCP > 0.0);
        }

        [XmlIgnore]
        public double AliquotaFCP { get; set; }

        [XmlElement("pFCP")]
        public string AliquotaFCPString
        {
            get
            {
                return AliquotaFCP.ToString("0.0000").Replace(",", ".");
            }
            set { AliquotaFCP = double.Parse(value.Replace(".", ","), AppDefaults.CultureInfoDefault); }
        }
        public bool ShouldSerializeAliquotaFCPString()
        {
            return (ValorBCFCP > 0.0) && (AliquotaFCP > 0.0) && (ValorFCP > 0.0);
        }

        [XmlIgnore]
        public double ValorFCP { get; set; }

        [XmlElement("vFCP")]
        public string ValorFCPString
        {
            get
            {
                return ValorFCP.ToString("0.00").Replace(",", ".");
            }
            set { ValorFCP = double.Parse(value.Replace(".", ","), AppDefaults.CultureInfoDefault); }
        }
        public bool ShouldSerializeValorFCPString()
        {
            return (ValorBCFCP > 0.0) && (AliquotaFCP > 0.0) && (ValorFCP > 0.0);
        }
    }
}
