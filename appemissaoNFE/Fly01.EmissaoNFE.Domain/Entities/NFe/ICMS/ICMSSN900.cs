using Fly01.EmissaoNFE.Domain.Enums;
using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFe.ICMS
{
    [XmlRoot(ElementName = "ICMSSN900")]
    public class ICMSSN900 : ICMS
    {

        public ICMSSN900()
        {

        }
        public ICMSSN900(OrigemMercadoria origemMercadoria, CSOSN codigoSituacaoOperacao) : base(origemMercadoria, codigoSituacaoOperacao)
        {
        }

        [XmlElement(ElementName = "modBC", IsNullable = true)]
        public ModalidadeDeterminacaoBCICMS? ModalidadeBC { get; set; }

        public bool ShouldSerializeModalidadeBC()
        {
            return ModalidadeBC.HasValue & ModalidadeBC > 0;
        }

        [XmlElement(ElementName = "pRedBC", IsNullable = true)]
        public double? PercentualReducaoBC { get; set; }

        public bool ShouldSerializePercentualReducaoBC()
        {
            return PercentualReducaoBC.HasValue & PercentualReducaoBC > 0;
        }

        [XmlElement(ElementName = "vBC", IsNullable = true)]
        public double? ValorBC { get; set; }

        public bool ShouldSerializeValorBC()
        {
            return ValorBC.HasValue & ValorBC > 0;
        }

        [XmlElement(ElementName = "pICMS", IsNullable = true)]
        public double? AliquotaICMS { get; set; }

        public bool ShouldSerializeAliquotaICMS()
        {
            return AliquotaICMS.HasValue & AliquotaICMS > 0;
        }

        [XmlElement(ElementName = "vICMS", IsNullable = true)]
        public double? ValorICMS { get; set; }

        public bool ShouldSerializeValorICMS()
        {
            return ValorICMS.HasValue & ValorICMS > 0;
        }

        [XmlElement(ElementName = "modBCST", IsNullable = true)]
        public ModalidadeDeterminacaoBCICMSST? ModalidadeBCST { get; set; }

        public bool ShouldSerializeModalidadeBCST()
        {
            return ModalidadeBCST.HasValue & ModalidadeBCST > 0;
        }

        [XmlElement(ElementName = "pMVAST", IsNullable = true)]
        public double? PercentualMargemValorAdicionadoST { get; set; }

        public bool ShouldSerializePercentualMargemValorAdicionadoST()
        {
            return PercentualMargemValorAdicionadoST.HasValue & PercentualMargemValorAdicionadoST > 0;
        }

        [XmlElement(ElementName = "pRedBCST", IsNullable = true)]
        public double? PercentualReducaoBCST { get; set; }

        public bool ShouldSerializePercentualReducaoBCST()
        {
            return PercentualReducaoBCST.HasValue & PercentualReducaoBCST > 0;
        }

        [XmlElement(ElementName = "vBCST", IsNullable = true)]
        public double? ValorBCST { get; set; }

        public bool ShouldSerializeValorBCST()
        {
            return ValorBCST.HasValue & ValorBCST > 0;
        }

        [XmlElement(ElementName = "pICMSST", IsNullable = true)]
        public double? AliquotaICMSST { get; set; }

        public bool ShouldSerializeAliquotaICMSST()
        {
            return AliquotaICMSST.HasValue & AliquotaICMSST > 0;
        }

        [XmlElement(ElementName = "vICMSST", IsNullable = true)]
        public double? ValorICMSST { get; set; }

        public bool ShouldSerializeValorICMSST()
        {
            return ValorICMSST.HasValue & ValorICMSST > 0;
        }

        [XmlElement(ElementName = "vBCSTRet", IsNullable = true)]
        public double? ValorBCSTRetido { get; set; }

        public bool ShouldSerializeValorBCSTRetido()
        {
            return ValorBCSTRetido.HasValue & ValorBCSTRetido > 0;
        }

        [XmlElement(ElementName = "vICMSSTRet", IsNullable = true)]
        public double? ValorICMSSTRetido { get; set; }

        public bool ShouldSerializeValorICMSSTRetido()
        {
            return ValorICMSSTRetido.HasValue & ValorICMSSTRetido > 0;
        }

        [XmlElement(ElementName = "vBCSTDest", IsNullable = true)]
        public double? ValorBCSTDestino { get; set; }

        public bool ShouldSerializeValorBCSTDestino()
        {
            return ValorBCSTDestino.HasValue & ValorBCSTDestino > 0;
        }

        [XmlElement(ElementName = "vICMSSTDest", IsNullable = true)]
        public double? ValorICMSSTUFDestino { get; set; }

        public bool ShouldSerializeValorICMSSTUFDestino()
        {
            return ValorICMSSTUFDestino.HasValue & ValorICMSSTUFDestino > 0;
        }

        [XmlElement(ElementName = "motDesICMS", IsNullable = true)]
        public int? MotivoDesoneracaoICMS { get; set; }

        public bool ShouldSerializeMotivoDesoneracaoICMS()
        {
            return MotivoDesoneracaoICMS.HasValue & MotivoDesoneracaoICMS > 0;
        }

        [XmlElement(ElementName = "pBCOp", IsNullable = true)]
        public double? PercentualBCop { get; set; }

        public bool ShouldSerializePercentualBCop()
        {
            return PercentualBCop.HasValue & PercentualBCop > 0;
        }

        [XmlElement(ElementName = "UFST", IsNullable = true)]
        public string UF { get; set; }

        public bool ShouldSerializeUF()
        {
            return !string.IsNullOrEmpty(UF) & !string.IsNullOrWhiteSpace(UF);
        }

        [XmlElement(ElementName = "pCredSN", IsNullable = true)]
        public double? AliquotaAplicavelCalculoCreditoSN { get; set; }

        public bool ShouldSerializeAliquotaAplicavelCalculoCreditoSN()
        {
            return AliquotaAplicavelCalculoCreditoSN.HasValue & AliquotaAplicavelCalculoCreditoSN > 0;
        }

        [XmlElement(ElementName = "vCredICMSSN", IsNullable = true)]
        public double? ValorCreditoICMS { get; set; }

        public bool ShouldSerializeValorCreditoICMS()
        {
            return ValorCreditoICMS.HasValue & ValorCreditoICMS > 0;
        }
    }
}
