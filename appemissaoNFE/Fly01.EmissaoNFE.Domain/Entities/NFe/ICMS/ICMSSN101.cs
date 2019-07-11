using Fly01.Core;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.EmissaoNFE.Domain.Enums;
using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFe.ICMS
{
    [XmlRoot("ICMSSN101")]
    public class ICMSSN101 : ICMS 
    {
        public ICMSSN101()
        {

        }
        public ICMSSN101(OrigemMercadoria origemMercadoria, TipoTributacaoICMS codigoSituacaoOperacao,TipoCRT tipoCRT) : base(origemMercadoria,codigoSituacaoOperacao, tipoCRT)
        {
        }

        [XmlIgnore]
        public double AliquotaAplicavelCalculoCreditoSN { get; set; }

        [XmlElement("pCredSN")]
        public string AliquotaAplicavelCalculoCreditoSNString
        {
            get
            {
                return AliquotaAplicavelCalculoCreditoSN.ToString("0.00").Replace(",", ".");
            }
            set { AliquotaAplicavelCalculoCreditoSN = double.Parse(value.Replace(".", ","), AppDefaults.CultureInfoDefault); }
        }

        [XmlIgnore]
        public double ValorCreditoICMS { get; set; }

        [XmlElement("vCredICMSSN")]
        public string ValorCreditoICMSString
        {
            get
            {
                return ValorCreditoICMS.ToString("0.00").Replace(",", ".");
            }
            set { ValorCreditoICMS = double.Parse(value.Replace(".", ","), AppDefaults.CultureInfoDefault); }
        }
    }
}
