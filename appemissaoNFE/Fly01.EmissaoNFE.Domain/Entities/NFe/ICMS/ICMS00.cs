using Fly01.Core;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.EmissaoNFE.Domain.Enums;
using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFe.ICMS
{
    [XmlRoot("ICMS00")]
    public class ICMS00 : ICMS 
    {
        public ICMS00()
        {

        }
        public ICMS00(OrigemMercadoria origemMercadoria, TipoTributacaoICMS codigoSituacaoOperacao,TipoCRT tipoCRT) : base(origemMercadoria,codigoSituacaoOperacao, tipoCRT)
        {
        }      

        [XmlElement(ElementName = "modBC")]
        public ModalidadeDeterminacaoBCICMS ModalidadeBC { get; set; }

        [XmlIgnore]
        public double? ValorBC { get; set; }

        [XmlElement("vBC")]
        public string ValorBCString
        {
            get
            {
                return ValorBC.HasValue && ValorBC > 0 ? ValorBC.Value.ToString("0.00").Replace(",", ".") : "0.00";
            }
            set { ValorBC = double.Parse(value.Replace(".", ","), AppDefaults.CultureInfoDefault); }
        }

        [XmlIgnore]
        public double? AliquotaICMS { get; set; }

        [XmlElement("pICMS")]
        public string AliquotaICMSString
        {
            get
            {
                return AliquotaICMS.HasValue && AliquotaICMS > 0 ? AliquotaICMS.Value.ToString("0.00").Replace(",", ".") : "0.00";
            }
            set { AliquotaICMS = double.Parse(value.Replace(".", ","), AppDefaults.CultureInfoDefault); }
        }

        [XmlIgnore]
        public double? ValorICMS { get; set; }

        [XmlElement("vICMS")]
        public string ValorICMSString
        {
            get
            {
                return ValorICMS.HasValue && ValorICMS > 0 ? ValorICMS.Value.ToString("0.00").Replace(",", ".") : "0.00";
            }
            set { ValorICMS = double.Parse(value.Replace(".", ","), AppDefaults.CultureInfoDefault); }
        }

        [XmlIgnore]
        public double? ValorBCSTDestino { get; set; }

        [XmlElement(ElementName = "vBCSTDest", IsNullable = true)]
        public string ValorBCSTDestinoString
        {
            get
            {
                return ValorBCSTDestino.HasValue && ValorBCSTDestino > 0 ? ValorBCSTDestino.Value.ToString("0.00").Replace(",", ".") : "0.00";
            }
            set { ValorBCSTDestino = double.Parse(value.Replace(".", ","), AppDefaults.CultureInfoDefault); }
        }

        public bool ShouldSerializeValorBCSTDestinoString()
        {
            return ValorBCSTDestino.HasValue && ValorBCSTDestino.Value > 0;
        }

        [XmlIgnore]
        public double? ValorICMSSTUFDestino { get; set; }

        [XmlElement(ElementName = "vICMSSTDest", IsNullable = true)]
        public string ValorICMSSTUFDestinoString
        {
            get
            {
                return ValorICMSSTUFDestino.HasValue && ValorICMSSTUFDestino > 0 ? ValorICMSSTUFDestino.Value.ToString("0.00").Replace(",", ".") : "0.00";
            }
            set { ValorICMSSTUFDestino = double.Parse(value.Replace(".", ","), AppDefaults.CultureInfoDefault); }
        }

        public bool ShouldSerializeValorICMSSTUFDestinoString()
        {
            return ValorICMSSTUFDestino.HasValue && ValorICMSSTUFDestino.Value > 0;
        }

        [XmlIgnore]
        public double? AliquotaFCP { get; set; }
        [XmlElement(ElementName = "pFCP", IsNullable = true)]
        public string AliquotaFCPString
        {
            get
            {
                return (AliquotaFCP.HasValue && AliquotaFCP > 0.0) ? AliquotaFCP.Value.ToString("0.0000").Replace(",", ".") : "0.0000";
            }
            set { AliquotaFCP = double.Parse(value); }
        }
        public bool ShouldSerializeAliquotaFCPString()
        {
            return ((AliquotaFCP.HasValue && AliquotaFCP.Value > 0.0) && (ValorFCP.HasValue && ValorFCP.Value > 0.0));
        }

        [XmlIgnore]
        public double? ValorFCP { get; set; }
        [XmlElement(ElementName = "vFCP", IsNullable = true)]
        public string ValorFCPString
        {
            get
            {
                return (ValorFCP.HasValue && ValorFCP > 0.0) ? ValorFCP.Value.ToString("0.00").Replace(",", ".") : "0.00";
            }
            set { ValorFCP = double.Parse(value); }
        }
        public bool ShouldSerializeValorFCPString()
        {
            return ((AliquotaFCP.HasValue && AliquotaFCP.Value > 0.0) && (ValorFCP.HasValue && ValorFCP.Value > 0.0));
        }


    }
}
