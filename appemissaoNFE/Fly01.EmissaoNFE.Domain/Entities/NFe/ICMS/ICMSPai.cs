using Fly01.Core.Entities.Domains.Enum;
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

                    ICMS = new ICMSSN101(OrigemMercadoria, CodigoSituacaoOperacao,TipoCRT)
                    {
                        AliquotaAplicavelCalculoCreditoSN = AliquotaAplicavelCalculoCreditoSN.Value,
                        ValorCreditoICMS = ValorCreditoICMS.Value
                    };
                    break;

                case "102"://Tributada pelo Simples Nacional sem permissão de crédito.
                    ICMS = new ICMSSN102(OrigemMercadoria, CodigoSituacaoOperacao, TipoCRT);
                    break;

                case "103"://Isenção do ICMS no Simples Nacional para faixa de receita bruta.
                    ICMS = new ICMSSN102(OrigemMercadoria, CodigoSituacaoOperacao, TipoCRT);
                    break;

                case "300"://Imune.
                    ICMS = new ICMSSN102(OrigemMercadoria, CodigoSituacaoOperacao, TipoCRT);
                    break;
                case "400"://Não tributada pelo Simples Nacional. 
                    ICMS = new ICMSSN102(OrigemMercadoria, CodigoSituacaoOperacao, TipoCRT);
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

                    //FCP
                    ToValidate.Add(NewKeyValuePair("BaseFCPST", BaseFCPST));
                    ToValidate.Add(NewKeyValuePair("AliquotaFCPST", AliquotaFCPST));
                    ToValidate.Add(NewKeyValuePair("ValorFCPST", ValorFCPST));

                    DoTheValidation();

                    ICMS = new ICMSSN201(OrigemMercadoria, CodigoSituacaoOperacao, TipoCRT)
                    {
                        ModalidadeBCST = ModalidadeBCST.Value,
                        PercentualMargemValorAdicionadoST = PercentualMargemValorAdicionadoST.Value,
                        PercentualReducaoBCST = PercentualReducaoBCST,
                        ValorBCST = ValorBCST.Value,
                        AliquotaICMSST = AliquotaICMSST.Value,
                        ValorICMSST = ValorICMSST.Value,
                        AliquotaAplicavelCalculoCreditoSN = AliquotaAplicavelCalculoCreditoSN.Value,
                        ValorCreditoICMS = ValorCreditoICMS.Value,
                        BaseFCPST = BaseFCPST,
                        AliquotaFCPST = AliquotaFCPST,
                        ValorFCPST = ValorFCPST
                    };

                    break;

                case "202"://Tributada pelo Simples Nacional sem permissão de crédito e com cobrança do ICMS por substituição tributária. 
                    ToValidate.Add(NewKeyValuePair("ModalidadeBCST", (int?)ModalidadeBCST));
                    ToValidate.Add(NewKeyValuePair("PercentualMargemValorAdicionadoST", PercentualMargemValorAdicionadoST));
                    ToValidate.Add(NewKeyValuePair("PercentualReducaoBCST", PercentualReducaoBCST));
                    ToValidate.Add(NewKeyValuePair("ValorBCST", ValorBCST));
                    ToValidate.Add(NewKeyValuePair("AliquotaICMSST", AliquotaICMSST));
                    ToValidate.Add(NewKeyValuePair("ValorICMSST", ValorICMSST));

                    //FCP
                    ToValidate.Add(NewKeyValuePair("BaseFCPST", BaseFCPST));
                    ToValidate.Add(NewKeyValuePair("AliquotaFCPST", AliquotaFCPST));
                    ToValidate.Add(NewKeyValuePair("ValorFCPST", ValorFCPST));

                    DoTheValidation();

                    ICMS = new ICMSSN202(OrigemMercadoria, CodigoSituacaoOperacao, TipoCRT)
                    {
                        ModalidadeBCST = ModalidadeBCST.Value,
                        PercentualMargemValorAdicionadoST = PercentualMargemValorAdicionadoST.Value,
                        PercentualReducaoBCST = PercentualReducaoBCST,
                        ValorBCST = ValorBCST.Value,
                        AliquotaICMSST = AliquotaICMSST.Value,
                        ValorICMSST = ValorICMSST.Value,
                        BaseFCPST = BaseFCPST,
                        AliquotaFCPST = AliquotaFCPST,
                        ValorFCPST = ValorFCPST
                    };

                    break;
                case "203"://Isenção do ICMS no Simples Nacional para faixa de receita bruta e com cobrança do ICMS por substituição tributária.
                    ToValidate.Add(NewKeyValuePair("ModalidadeBCST", (int?)ModalidadeBCST));
                    ToValidate.Add(NewKeyValuePair("PercentualMargemValorAdicionadoST", PercentualMargemValorAdicionadoST));
                    ToValidate.Add(NewKeyValuePair("PercentualReducaoBCST", PercentualReducaoBCST));
                    ToValidate.Add(NewKeyValuePair("ValorBCST", ValorBCST));
                    ToValidate.Add(NewKeyValuePair("AliquotaICMSST", AliquotaICMSST));
                    ToValidate.Add(NewKeyValuePair("ValorICMSST", ValorICMSST));

                    //FCP
                    ToValidate.Add(NewKeyValuePair("BaseFCPST", BaseFCPST));
                    ToValidate.Add(NewKeyValuePair("AliquotaFCPST", AliquotaFCPST));
                    ToValidate.Add(NewKeyValuePair("ValorFCPST", ValorFCPST));

                    DoTheValidation();

                    ICMS = new ICMSSN202(OrigemMercadoria, CodigoSituacaoOperacao, TipoCRT)
                    {
                        ModalidadeBCST = ModalidadeBCST.Value,
                        PercentualMargemValorAdicionadoST = PercentualMargemValorAdicionadoST.Value,
                        PercentualReducaoBCST = PercentualReducaoBCST,
                        ValorBCST = ValorBCST.Value,
                        AliquotaICMSST = AliquotaICMSST.Value,
                        ValorICMSST = ValorICMSST.Value,
                        BaseFCPST = BaseFCPST,
                        AliquotaFCPST = AliquotaFCPST,
                        ValorFCPST = ValorFCPST
                    };

                    break;

                case "500"://ICMS cobrado anteriormente por substituição tributária (substituído) ou por antecipação.
                    ToValidate.Add(NewKeyValuePair("ValorBCSTRetido", ValorBCSTRetido));
                    ToValidate.Add(NewKeyValuePair("ValorICMSSTRetido", ValorICMSSTRetido));
                    
                    ToValidate.Add(NewKeyValuePair("BaseFCPSTRetido", BaseFCPSTRetido));
                    ToValidate.Add(NewKeyValuePair("AliquotaFCPSTRetido", AliquotaFCPSTRetido));
                    ToValidate.Add(NewKeyValuePair("ValorFCPSTRetido", ValorFCPSTRetido));
                    ToValidate.Add(NewKeyValuePair("AliquotaConsumidorFinal", AliquotaConsumidorFinal));

                    DoTheValidation();

                    ICMS = new ICMSSN500(OrigemMercadoria, CodigoSituacaoOperacao, TipoCRT)
                    {
                        ValorBCSTRetido = ValorBCSTRetido.Value,
                        ValorICMSSTRetido = ValorICMSSTRetido.Value,
                        BaseFCPSTRetido = BaseFCPSTRetido,
                        AliquotaFCPSTRetido = AliquotaFCPSTRetido,
                        ValorFCPSTRetido = ValorFCPSTRetido,
                        AliquotaConsumidorFinal = AliquotaConsumidorFinal.Value
                    };
                    break;
                case "900"://Outros
                    DoTheValidation();

                    ICMS = new ICMSSN900(OrigemMercadoria, CodigoSituacaoOperacao, TipoCRT)
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
                        PercentualReducaoBC = PercentualReducaoBC,
                        BaseFCPST = BaseFCPST,
                        AliquotaFCPST = AliquotaFCPST,
                        ValorFCPST = ValorFCPST
                    };
                    break;

                case "0"://Integral

                    ToValidate.Add(NewKeyValuePair("ModalidadeBC", (int?)ModalidadeBC));
                 //   ToValidate.Add(NewKeyValuePair("ValorICMS", ValorICMS));
                 //   ToValidate.Add(NewKeyValuePair("AliquotaICMS", AliquotaICMS));
                  //  ToValidate.Add(NewKeyValuePair("ValorBC", ValorBC));


                    DoTheValidation();

                    ICMS = new ICMS00(OrigemMercadoria, CodigoSituacaoOperacao, TipoCRT)
                    {

                        ModalidadeBC = ModalidadeBC.Value,
                        ValorBC = ValorBC.Value,
                        ValorICMS = ValorICMS.Value,
                        AliquotaICMS = AliquotaICMS.Value,
                        AliquotaFCP = AliquotaFCP,
                        ValorFCP = ValorFCP,
                        ValorICMSSTUFDestino = ValorICMSSTUFDestino,
                        ValorBCSTDestino = ValorBCSTDestino
                    };
                    break;
              
               
                case "10"://Com substituiçao

                    ToValidate.Add(NewKeyValuePair("ModalidadeBC", (int?)ModalidadeBC));
                    ToValidate.Add(NewKeyValuePair("ModalidadeBCST", (int?)ModalidadeBCST));
                    ToValidate.Add(NewKeyValuePair("ValorICMS", ValorICMS));
                    ToValidate.Add(NewKeyValuePair("ValorICMSST", ValorICMSST));
                    ToValidate.Add(NewKeyValuePair("AliquotaICMS", AliquotaICMS));
                    ToValidate.Add(NewKeyValuePair("AliquotaICMSST", AliquotaICMSST));
                    ToValidate.Add(NewKeyValuePair("ValorBC", ValorBC));
                    ToValidate.Add(NewKeyValuePair("ValorBCST", ValorBCST));
                    ToValidate.Add(NewKeyValuePair("PercentualMargemValorAdicionadoST", PercentualMargemValorAdicionadoST));

                    //FCP
                    ToValidate.Add(NewKeyValuePair("BaseFCPST", BaseFCPST));
                    ToValidate.Add(NewKeyValuePair("AliquotaFCPST", AliquotaFCPST));
                    ToValidate.Add(NewKeyValuePair("ValorFCPST", ValorFCPST));



                    DoTheValidation();

                    ICMS = new ICMS10(OrigemMercadoria, CodigoSituacaoOperacao, TipoCRT)
                    {
                        ModalidadeBC = ModalidadeBC.Value,
                        ModalidadeBCST = ModalidadeBCST.Value,
                        ValorBC = ValorBC.Value,
                        ValorBCST = ValorBCST.Value,
                        ValorICMS = ValorICMS.Value,
                        ValorICMSST = ValorICMSST.Value,
                        AliquotaICMS = AliquotaICMS.Value,
                        AliquotaICMSST = AliquotaICMSST.Value,
                        BaseFCPST = BaseFCPST,
                        AliquotaFCPST = AliquotaFCPST,
                        ValorFCPST = ValorFCPST,
                        PercentualMargemValorAdicionadoST = PercentualMargemValorAdicionadoST.Value,                       
                    };
                    break;


                case "20"://Com reduçao na base 

                    ToValidate.Add(NewKeyValuePair("ModalidadeBC", (int?)ModalidadeBC));
                    ToValidate.Add(NewKeyValuePair("ValorICMS", ValorICMS));
                    ToValidate.Add(NewKeyValuePair("AliquotaICMS", AliquotaICMS));
                    ToValidate.Add(NewKeyValuePair("ValorBC", ValorBC));
                    ToValidate.Add(NewKeyValuePair("PercentualReducaoBC", PercentualReducaoBC));                   


                    DoTheValidation();

                    ICMS = new ICMS20(OrigemMercadoria, CodigoSituacaoOperacao, TipoCRT)
                    {

                        ModalidadeBC = ModalidadeBC.Value,
                        ValorBC = ValorBC.Value,
                        ValorICMS = ValorICMS.Value,
                        AliquotaICMS = AliquotaICMS.Value,
                        PercentualReducaoBC = PercentualReducaoBC.Value,
                        MotivoDesoneracaoICMS = MotivoDesoneracaoICMS.Value,
                        AliquotaFCP = AliquotaFCP.Value,
                        ValorFCP = ValorFCP.Value,

                    };
                    break;

                case "30"://Com reduçao de st

                 
                    ToValidate.Add(NewKeyValuePair("ModalidadeBCST", (int?)ModalidadeBCST));
                    ToValidate.Add(NewKeyValuePair("ValorBCST", ValorBCST));
                    ToValidate.Add(NewKeyValuePair("AliquotaICMSST", AliquotaICMSST));
                    ToValidate.Add(NewKeyValuePair("ValorICMSST", ValorICMSST));
                    ToValidate.Add(NewKeyValuePair("PercentualMargemValorAdicionadoST", PercentualMargemValorAdicionadoST));
             
                    DoTheValidation();

                    ICMS = new ICMS30(OrigemMercadoria, CodigoSituacaoOperacao, TipoCRT)
                    {
                        
                        ModalidadeBCST = ModalidadeBCST.Value,
                        ValorBCST = ValorBCST.Value,                 
                        ValorICMSST = ValorICMSST.Value,                       
                        AliquotaICMSST = AliquotaICMSST.Value,
                        BaseFCPST = BaseFCPST.Value,
                        AliquotaFCPST = AliquotaFCPST.Value,
                        ValorFCPST = ValorFCPST.Value,
                        PercentualMargemValorAdicionadoST = PercentualMargemValorAdicionadoST.Value,
                        PercentualReducaoBCST = PercentualReducaoBCST.Value,
                        MotivoDesoneracaoICMS = MotivoDesoneracaoICMS.Value,

                    };
                    break;

                case "40"://Isento.
                    ICMS = new ICMS40(OrigemMercadoria, CodigoSituacaoOperacao, TipoCRT);
                    {
                        MotivoDesoneracaoICMS = MotivoDesoneracaoICMS;
                    };
                    break;
                case "41"://Não tributado. 
                    ICMS = new ICMS40(OrigemMercadoria, CodigoSituacaoOperacao, TipoCRT);
                    break;
                case "50"://Com suspensao.
                    ICMS = new ICMS50(OrigemMercadoria, CodigoSituacaoOperacao, TipoCRT);
                    {
                        MotivoDesoneracaoICMS = MotivoDesoneracaoICMS.Value;
                    };
                    break;
                case "51"://Diferimento. 
                    ICMS = new ICMS51(OrigemMercadoria, CodigoSituacaoOperacao, TipoCRT);
                    break;

                case "60"://ICMS ST Retido anteriormente
                /*    ToValidate.Add(NewKeyValuePair("ValorBCSTRetido", ValorBCSTRetido));
                    ToValidate.Add(NewKeyValuePair("ValorICMSSTRetido", ValorICMSSTRetido));
                    ToValidate.Add(NewKeyValuePair("BaseFCPSTRetido", BaseFCPSTRetido));
                    ToValidate.Add(NewKeyValuePair("AliquotaFCPSTRetido", AliquotaFCPSTRetido));
                    ToValidate.Add(NewKeyValuePair("ValorFCPSTRetido", ValorFCPSTRetido));
                    ToValidate.Add(NewKeyValuePair("AliquotaConsumidorFinal", AliquotaConsumidorFinal));
                    */
                    DoTheValidation();

                    ICMS = new ICMS60(OrigemMercadoria, CodigoSituacaoOperacao, TipoCRT)
                    {
                        ValorBCSTRetido = ValorBCSTRetido.Value,
                        ValorICMSSTRetido = ValorICMSSTRetido.Value,
                        BaseFCPSTRetido = BaseFCPSTRetido,
                       // ValorICMSSubstituto = ValorICMSSubstituto,
                        AliquotaFCPSTRetido = AliquotaFCPSTRetido,
                        ValorFCPSTRetido = ValorFCPSTRetido,
                        AliquotaConsumidorFinal = AliquotaConsumidorFinal,
                    };
                    break;

                case "70"://Com reducaçao da base e cobrado por substituiçao

                    ToValidate.Add(NewKeyValuePair("ModalidadeBC", (int?)ModalidadeBC));
                    ToValidate.Add(NewKeyValuePair("ModalidadeBCST", (int?)ModalidadeBCST));
                    ToValidate.Add(NewKeyValuePair("ValorICMS", ValorICMS));
                    ToValidate.Add(NewKeyValuePair("ValorICMSST", ValorICMSST));
                    ToValidate.Add(NewKeyValuePair("AliquotaICMS", AliquotaICMS));
                    ToValidate.Add(NewKeyValuePair("AliquotaICMSST", AliquotaICMSST));
                    ToValidate.Add(NewKeyValuePair("ValorBC", ValorBC));
                    ToValidate.Add(NewKeyValuePair("ValorBCST", ValorBCST));
                    ToValidate.Add(NewKeyValuePair("PercentualMargemValorAdicionadoST", PercentualMargemValorAdicionadoST));

                    DoTheValidation();

                    ICMS = new ICMS70(OrigemMercadoria, CodigoSituacaoOperacao, TipoCRT)
                    {
                        ModalidadeBC = ModalidadeBC.Value,
                        ModalidadeBCST = ModalidadeBCST.Value,
                        ValorBC = ValorBC.Value,
                        ValorBCST = ValorBCST.Value,
                        ValorICMS = ValorICMS.Value,
                        ValorICMSST = ValorICMSST.Value,
                        AliquotaICMS = AliquotaICMS.Value,
                        AliquotaICMSST = AliquotaICMSST.Value,
                        BaseFCPST = BaseFCPST.Value,
                        AliquotaFCPST = AliquotaFCPST.Value,
                        ValorFCPST = ValorFCPST.Value,
                        PercentualMargemValorAdicionadoST = PercentualMargemValorAdicionadoST.Value,
                        PercentualReducaoBC = PercentualReducaoBC.Value,
                        PercentualReducaoBCST = PercentualReducaoBCST.Value,
                    };
                    break;

                case "90"://Outros
                    DoTheValidation();

                    ICMS = new ICMS90(OrigemMercadoria, CodigoSituacaoOperacao, TipoCRT)
                    {                        
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
                        ValorICMSSTUFDestino = ValorICMSSTUFDestino,
                        PercentualReducaoBC = PercentualReducaoBC,
                        BaseFCPST = BaseFCPST,
                        AliquotaFCPST = AliquotaFCPST,
                        ValorFCPST = ValorFCPST
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
        public TipoCRT TipoCRT { get; set; }

        [XmlIgnore]
        public TipoTributacaoICMS CodigoSituacaoOperacao { get; set; }
        
        [XmlIgnore]
        public ModalidadeDeterminacaoBCICMS? ModalidadeBC { get; set; }

        [XmlIgnore]
        public double? PercentualReducaoBC { get; set; }

        [XmlIgnore]
        public double? ValorBC { get; set; }

        [XmlIgnore]
        public double? AliquotaICMS { get; set; }

        [XmlIgnore]
        public double? ValorBCST { get; set; }

        [XmlIgnore]
        public double? ValorICMS { get; set; }

        [XmlIgnore]
        public ModalidadeDeterminacaoBCICMSST? ModalidadeBCST { get; set; }

        [XmlIgnore]
        public double? PercentualMargemValorAdicionadoST { get; set; }

        [XmlIgnore]
        public double? PercentualReducaoBCST { get; set; }        

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
        public string UF { get; set; }

        [XmlIgnore]
        public double? AliquotaAplicavelCalculoCreditoSN { get; set; }

        [XmlIgnore]
        public double? ValorCreditoICMS { get; set; }

        [XmlIgnore] 
        public double? ValorICMSSubstituto{ get; set; }
        
        #region FCP

        [XmlIgnore]
        public double? BaseFCP { get; set; }

        [XmlIgnore]
        public double? AliquotaFCP { get; set; }

        [XmlIgnore]
        public double? ValorFCP { get; set; }

        [XmlIgnore]
        public double? BaseFCPST { get; set; }

        [XmlIgnore]
        public double? AliquotaFCPST { get; set; }

        [XmlIgnore]
        public double? ValorFCPST { get; set; }

        [XmlIgnore]
        public double? BaseFCPSTRetido { get; set; }

        [XmlIgnore]
        public double? AliquotaFCPSTRetido { get; set; }

        [XmlIgnore]
        public double? ValorFCPSTRetido { get; set; }

        [XmlIgnore]
        public double? AliquotaConsumidorFinal { get; set; }

        #endregion FCP

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
