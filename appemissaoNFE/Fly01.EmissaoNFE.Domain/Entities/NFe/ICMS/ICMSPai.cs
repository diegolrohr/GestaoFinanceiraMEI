using Fly01.EmissaoNFE.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFe.ICMS
{

    public class ICMSPai
    {
        private IList<KeyValuePair<string, double?>> ToValidate = new List<KeyValuePair<string, double?>>();
        private static string ErrOrigemCSOSN = @"Devem ser informados a origem e o CSOSN.";
        private static string ErrNotInformed = @"Os seguintes campos obrigatórios não foram informados: ";


        public ICMSPai() { }

        public void DoTheIcms()
        {

            if (string.IsNullOrWhiteSpace(((int)OrigemMercadoria).ToString()) || string.IsNullOrWhiteSpace(CodigoSituacaoOperacao.ToString()))
                throw new Exception(ErrOrigemCSOSN);

            switch (((int)CodigoSituacaoOperacao).ToString())
            {
                case "101"://Tributada pelo Simples Nacional com permissão de crédito.
                    ToValidate.Add(NewKeyValuePair("AliquotaAplicavelCalculoCreditoSN", AliquotaAplicavelCalculoCreditoSN));
                    ToValidate.Add(NewKeyValuePair("ValorCreditoICMS", ValorCreditoICMS));

                    DoTheValidation();

                    ICMS = new ICMSSN101(OrigemMercadoria, CodigoSituacaoOperacao)
                    {
                        AliquotaAplicavelCalculoCreditoSN = AliquotaAplicavelCalculoCreditoSN.Value,
                        ValorCreditoICMS = ValorCreditoICMS.Value
                    };
                    break;

                case "102"://Tributada pelo Simples Nacional sem permissão de crédito.
                    ICMS = new ICMSSN102(OrigemMercadoria, CodigoSituacaoOperacao);
                    break;

                case "103"://Isenção do ICMS no Simples Nacional para faixa de receita bruta.
                    ICMS = new ICMSSN103(OrigemMercadoria, CodigoSituacaoOperacao);
                    break;

                case "300"://Imune.
                    ICMS = new ICMSSN300(OrigemMercadoria, CodigoSituacaoOperacao);
                    break;
                case "400"://Não tributada pelo Simples Nacional. 
                    ICMS = new ICMSSN400(OrigemMercadoria, CodigoSituacaoOperacao);
                    break;

                case "201"://Tributada pelo Simples Nacional com permissão de crédito e com cobrança do ICMS por substituição tributária.
                    ToValidate.Add(NewKeyValuePair("ModalidadeBCST", (int?)ModalidadeBCST));
                    ToValidate.Add(NewKeyValuePair("PercentualMargemValorAdicionadoST", PercentualMargemValorAdicionadoST));
                    ToValidate.Add(NewKeyValuePair("PercentualReducaoBCST", PercentualReducaoBCST));
                    ToValidate.Add(NewKeyValuePair("ValorBCST", ValorBCST));
                    ToValidate.Add(NewKeyValuePair("AliquotaICMSST", AliquotaICMSST));
                    ToValidate.Add(NewKeyValuePair("ValorICMSST", ValorICMSST));
                    ToValidate.Add(NewKeyValuePair("AliquotaAplicavelCalculoCreditoSN", AliquotaAplicavelCalculoCreditoSN));
                    ToValidate.Add(NewKeyValuePair("ValorCreditoICMS", ValorCreditoICMS));

                    DoTheValidation();

                    ICMS = new ICMSSN201(OrigemMercadoria, CodigoSituacaoOperacao)
                    {
                        ModalidadeBCST = ModalidadeBCST.Value,
                        PercentualMargemValorAdicionadoST = PercentualMargemValorAdicionadoST.Value,
                        PercentualReducaoBCST = PercentualReducaoBCST,
                        ValorBCST = ValorBCST.Value,
                        AliquotaICMSST = AliquotaICMSST.Value,
                        ValorICMSST = ValorICMSST.Value,
                        AliquotaAplicavelCalculoCreditoSN = AliquotaAplicavelCalculoCreditoSN.Value,
                        ValorCreditoICMS = ValorCreditoICMS.Value
                    };

                    break;

                case "202"://Tributada pelo Simples Nacional sem permissão de crédito e com cobrança do ICMS por substituição tributária. 
                    ToValidate.Add(NewKeyValuePair("ModalidadeBCST", (int?)ModalidadeBCST));
                    ToValidate.Add(NewKeyValuePair("PercentualMargemValorAdicionadoST", PercentualMargemValorAdicionadoST));
                    ToValidate.Add(NewKeyValuePair("PercentualReducaoBCST", PercentualReducaoBCST));
                    ToValidate.Add(NewKeyValuePair("ValorBCST", ValorBCST));
                    ToValidate.Add(NewKeyValuePair("AliquotaICMSST", AliquotaICMSST));
                    ToValidate.Add(NewKeyValuePair("ValorICMSST", ValorICMSST));

                    DoTheValidation();

                    ICMS = new ICMSSN202(OrigemMercadoria, CodigoSituacaoOperacao)
                    {
                        ModalidadeBCST = ModalidadeBCST.Value,
                        PercentualMargemValorAdicionadoST = PercentualMargemValorAdicionadoST.Value,
                        PercentualReducaoBCST = PercentualReducaoBCST,
                        ValorBCST = ValorBCST.Value,
                        AliquotaICMSST = AliquotaICMSST.Value,
                        ValorICMSST = ValorICMSST.Value
                    };

                    break;
                case "203"://Isenção do ICMS no Simples Nacional para faixa de receita bruta e com cobrança do ICMS por substituição tributária.
                    ToValidate.Add(NewKeyValuePair("ModalidadeBCST", (int?)ModalidadeBCST));
                    ToValidate.Add(NewKeyValuePair("PercentualMargemValorAdicionadoST", PercentualMargemValorAdicionadoST));
                    ToValidate.Add(NewKeyValuePair("PercentualReducaoBCST", PercentualReducaoBCST));
                    ToValidate.Add(NewKeyValuePair("ValorBCST", ValorBCST));
                    ToValidate.Add(NewKeyValuePair("AliquotaICMSST", AliquotaICMSST));
                    ToValidate.Add(NewKeyValuePair("ValorICMSST", ValorICMSST));

                    DoTheValidation();

                    ICMS = new ICMSSN203(OrigemMercadoria, CodigoSituacaoOperacao)
                    {
                        ModalidadeBCST = ModalidadeBCST.Value,
                        PercentualMargemValorAdicionadoST = PercentualMargemValorAdicionadoST.Value,
                        PercentualReducaoBCST = PercentualReducaoBCST,
                        ValorBCST = ValorBCST.Value,
                        AliquotaICMSST = AliquotaICMSST.Value,
                        ValorICMSST = ValorICMSST.Value
                    };

                    break;

                case "500"://ICMS cobrado anteriormente por substituição tributária (substituído) ou por antecipação.
                    ToValidate.Add(NewKeyValuePair("ValorBCSTRetido", ValorBCSTRetido));
                    ToValidate.Add(NewKeyValuePair("ValorICMSSTRetido", ValorICMSSTRetido));

                    DoTheValidation();

                    ICMS = new ICMSSN500(OrigemMercadoria, CodigoSituacaoOperacao)
                    {
                        ValorBCSTRetido = ValorBCSTRetido.Value,
                        ValorICMSSTRetido = ValorICMSSTRetido.Value
                    };
                    break;
                case "900"://Outros
                    ICMS = new ICMSSN900(OrigemMercadoria, CodigoSituacaoOperacao)
                    {
                        AliquotaAplicavelCalculoCreditoSN = AliquotaAplicavelCalculoCreditoSN,
                        AliquotaICMS = AliquotaICMS,
                        AliquotaICMSST = AliquotaICMSST,
                        ModalidadeBC = ModalidadeBC,
                        ModalidadeBCST = ModalidadeBCST,
                        PercentualBCop = PercentualBCop,
                        PercentualReducaoBCST = PercentualReducaoBCST,
                        MotivoDesoneracaoICMS = MotivoDesoneracaoICMS,
                        ValorBC = ValorBC,
                        ValorBCST = ValorBCST,
                        ValorBCSTDestino = ValorBCSTDestino,
                        ValorBCSTRetido = ValorBCSTRetido,
                        PercentualMargemValorAdicionadoST = PercentualMargemValorAdicionadoST,
                        ValorICMSSTRetido = ValorICMSSTRetido,
                        ValorICMS = ValorICMS,
                        ValorICMSST = ValorICMSST,
                        ValorCreditoICMS = ValorCreditoICMS,
                        ValorICMSSTUFDestino = ValorICMSSTUFDestino,
                        PercentualReducaoBC = PercentualReducaoBC
                    };

                    break;
                default:
                    throw new NotImplementedException();
            }
        }


        public ICMS ICMS { get; set; }

        [XmlIgnore]
        public OrigemMercadoria OrigemMercadoria { get; set; }

        [XmlIgnore]
        public CSOSN CodigoSituacaoOperacao { get; set; }
        
        [XmlIgnore]
        public ModalidadeDeterminacaoBCICMS? ModalidadeBC { get; set; }

        [XmlIgnore]
        public double? PercentualReducaoBC { get; set; }

        [XmlIgnore]
        public double? ValorBC { get; set; }

        [XmlIgnore]
        public double? AliquotaICMS { get; set; }

        [XmlIgnore]
        public double? ValorICMS { get; set; }

        [XmlIgnore]
        public ModalidadeDeterminacaoBCICMSST? ModalidadeBCST { get; set; }

        [XmlIgnore]
        public double? PercentualMargemValorAdicionadoST { get; set; }

        [XmlIgnore]
        public double? PercentualReducaoBCST { get; set; }

        [XmlIgnore]
        public double? ValorBCST { get; set; }

        [XmlIgnore]
        public double? AliquotaICMSST { get; set; }

        [XmlIgnore]
        public double? ValorICMSST { get; set; }

        [XmlIgnore]
        public double? ValorBCSTRetido { get; set; }

        [XmlIgnore]
        public double? ValorICMSSTRetido { get; set; }

        [XmlIgnore]
        public double? ValorBCSTDestino { get; set; }

        [XmlIgnore]
        public double? ValorICMSSTUFDestino { get; set; }

        [XmlIgnore]
        public int? MotivoDesoneracaoICMS { get; set; }

        [XmlIgnore]
        public double? PercentualBCop { get; set; }

        [XmlIgnore]
        public string UF{ get; set; }

        [XmlIgnore]
        public double? AliquotaAplicavelCalculoCreditoSN { get; set; }

        [XmlIgnore]
        public double? ValorCreditoICMS { get; set; }
        

        private void DoTheValidation()
        {
            if (ToValidate.Any(e =>
            {
                return !e.Value.HasValue;
            }))
            {
                ThrowErrorNotInformed();
            }
        }

        public KeyValuePair<string, double?> NewKeyValuePair(string key, double? value)
        {
            return new KeyValuePair<string, double?>(key, value);
        }

        private void ThrowErrorNotInformed()
        {
            StringBuilder sb = new StringBuilder(ErrNotInformed);

            foreach (var field in ToValidate)
            {
                sb.Append(string.Format("{0},", field.Key));
            }

            sb.Length--;

            throw new Exception(sb.ToString());
        }
    }
}
