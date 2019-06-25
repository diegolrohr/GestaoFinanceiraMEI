using Fly01.Core.Helpers;
using Fly01.Core.Helpers.Attribute;
using Fly01.Core.Notifications;
using Fly01.EmissaoNFE.Domain.Entities.NFe;
using Fly01.EmissaoNFE.Domain.Enums;
using Fly01.EmissaoNFE.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fly01.EmissaoNFE.BL.Helpers.ValidaModelTransmissao.ValidaImpostoTransmissao
{
    public static class ValidaDetalheImpostoICMS
    {
        public static void ExecutaValidaImpostoICMS(Detalhe detalhe, TransmissaoVM entity, int nItemDetalhe, ItemTransmissaoVM item)
        {
            #region Validações da classe Imposto.ICMS

            if (detalhe.Imposto.ICMS == null)
                entity.Fail(true, new Error("Os dados de ICMS são obrigatórios. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.ICMS"));
            else
            {
                ValidarOrigemDaCategoria(detalhe, entity, nItemDetalhe);

                var Modalidade = EnumHelper.GetDataEnumValues(typeof(ModalidadeDeterminacaoBCICMS));
                var ModalidadeST = EnumHelper.GetDataEnumValues(typeof(ModalidadeDeterminacaoBCICMSST));

                ValidarPorCodigoSituacaoOperacao(detalhe, entity, nItemDetalhe, item, Modalidade, ModalidadeST);
            }
            #endregion
        }

        private static void ValidarPorCodigoSituacaoOperacao(Detalhe detalhe, TransmissaoVM entity, int nItemDetalhe, ItemTransmissaoVM item, List<SubtitleAttribute> Modalidade, List<SubtitleAttribute> ModalidadeST)
        {
            switch (((int)detalhe.Imposto.ICMS.CodigoSituacaoOperacao).ToString())
            {
                case "101":
                    ValidarSimplesNacionalComPermissaoCredito_101(detalhe, entity, nItemDetalhe);
                    break;

                case "102":
                    break;

                case "103":
                    break;

                case "201":
                    ValidarCasoSubstituicaoTributaria_201(detalhe, entity, nItemDetalhe, item, ModalidadeST);
                    break;

                case "202":
                    ValidarSubstituicaoTributaria_202(detalhe, entity, nItemDetalhe, item, ModalidadeST);
                    break;

                case "203":
                    ValidarSubstituicaoTributaria_203(detalhe, entity, nItemDetalhe, item, ModalidadeST);
                    break;

                case "300":
                    break;

                case "400":
                    break;

                case "500":
                    ValidarSubstituicaoTributaria_500(detalhe, entity, nItemDetalhe);
                    break;

                case "900":
                    ValidarSubstituicaoTributariaOutros_900(detalhe, entity, nItemDetalhe, item, Modalidade, ModalidadeST);
                    break;

                case "00":
                    ValidarLP(detalhe, entity, nItemDetalhe, item, Modalidade);
                    break;

                case "0":
                    ValidarLP(detalhe, entity, nItemDetalhe, item, Modalidade);
                    break;

                case "10":
                    ValidarSubstituicaoTributaria_202(detalhe, entity, nItemDetalhe, item, ModalidadeST);
                    break;

                case "20":
                    ValidarLP(detalhe, entity, nItemDetalhe, item, Modalidade);
                    break;

                case "30":
                    break;

                case "40":
                    break;

                case "41":
                    break;

                case "50":
                    break;

                case "51":
                    ValidarLP(detalhe, entity, nItemDetalhe, item, Modalidade);
                    break;

                case "60":
                    break;

                case "70":
                    break;

                case "90":
                    break;

                default:
                    entity.Fail(true, new Error("CSOSN inválido. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.ICMS.CodigoSituacaoOperacao"));
                    break;
            }
        }

        private static void ValidarSimplesNacionalComPermissaoCredito_101(Detalhe detalhe, TransmissaoVM entity, int nItemDetalhe)
        {
            entity.Fail(!detalhe.Imposto.ICMS.AliquotaAplicavelCalculoCreditoSN.HasValue,
                                    new Error("Alíquota aplicável de cálculo do crédito é obrigatória para CSOSN 101 e 201. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.ICMS.AliquotaAplicavelCalculoCreditoSN"));
            entity.Fail(!detalhe.Imposto.ICMS.ValorCreditoICMS.HasValue,
                                    new Error("Valor crédito do ICMS é obrigatório para CSOSN 101 e 201. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.ICMS.ValorCreditoICMS"));
        }

        private static void ValidarSubstituicaoTributariaOutros_900(Detalhe detalhe, TransmissaoVM entity, int nItemDetalhe, ItemTransmissaoVM item, List<SubtitleAttribute> Modalidade, List<SubtitleAttribute> ModalidadeST)
        {
            ValidarInformacoesDoCSOSNEICMSPossivelDeCredito(detalhe, entity, nItemDetalhe);

            var ICMSProprio = ValidarInformacoesCSOSNeICMSProprio(detalhe, entity, nItemDetalhe, Modalidade);

            ValidarInformacoesCSOSN_ICMSProprioEICMSST(detalhe, entity, nItemDetalhe, item, ModalidadeST, ICMSProprio);
        }

        private static void ValidarInformacoesCSOSN_ICMSProprioEICMSST(Detalhe detalhe, TransmissaoVM entity, int nItemDetalhe, ItemTransmissaoVM item, List<SubtitleAttribute> ModalidadeST, bool ICMSProprio)
        {
            if (IsICMSProprioEValoresDetalhesValidos(detalhe, ICMSProprio))
            {
                entity.Fail(!string.IsNullOrEmpty(detalhe.Imposto.ICMS.ModalidadeBCST.ToString()) && !ModalidadeST.Any(x => int.Parse(x.Value) == ((int)detalhe.Imposto.ICMS.ModalidadeBCST)),
                    new Error("Modalidade de determinação da base de cálculo inválida. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.ICMS.ModalidadeBCST"));
                entity.Fail(!detalhe.Imposto.ICMS.ValorBCST.HasValue,
                    new Error("Valor da base de cálculo do ICMS ST é obrigatório para CSOSN 201, 202 e 203. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.ICMS.ValorBCST"));
                entity.Fail(!detalhe.Imposto.ICMS.AliquotaICMSST.HasValue,
                    new Error("Alíquota da Substituição Tributária é requerida. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.ICMS.ValorBC"));
                entity.Fail(!detalhe.Imposto.ICMS.ValorICMSST.HasValue,
                    new Error("Valor da Substituição Tributária é requerido. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.ICMS.ValorBC"));

                if (item.Versao == "4.00")
                {
                    entity.Fail(!detalhe.Imposto.ICMS.BaseFCPST.HasValue,
                    new Error("Valor da Base de Cálculo do FCP retido por Substituição Tributária é obrigatório para CSOSN 201, 202, 203 e 900. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.ICMS.vBCFCPST"));
                    entity.Fail(!detalhe.Imposto.ICMS.AliquotaFCPST.HasValue,
                        new Error("Percentual do FCP retido por Substituição Tributária é obrigatório para CSOSN 201, 202, 203 e 900. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.ICMS.pFCPST"));
                    entity.Fail(!detalhe.Imposto.ICMS.ValorFCPST.HasValue,
                        new Error("Valor do FCP retido por Substituição Tributária é obrigatório para CSOSN 201, 202, 203 e 900. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.ICMS.vFCPST"));
                }
            }
        }

        private static bool IsICMSProprioEValoresDetalhesValidos(Detalhe detalhe, bool ICMSProprio)
        {
            return ICMSProprio &
                          (detalhe.Imposto.ICMS.PercentualMargemValorAdicionadoST.HasValue && detalhe.Imposto.ICMS.PercentualMargemValorAdicionadoST > 0) ||
                          (detalhe.Imposto.ICMS.PercentualReducaoBCST.HasValue && detalhe.Imposto.ICMS.PercentualReducaoBCST > 0) ||
                          (detalhe.Imposto.ICMS.ValorBCST.HasValue && detalhe.Imposto.ICMS.ValorBCST > 0) ||
                          (detalhe.Imposto.ICMS.AliquotaICMSST.HasValue && detalhe.Imposto.ICMS.AliquotaICMSST > 0) ||
                          (detalhe.Imposto.ICMS.ValorICMSST.HasValue && detalhe.Imposto.ICMS.ValorICMSST > 0);
        }

        private static bool ValidarInformacoesCSOSNeICMSProprio(Detalhe detalhe, TransmissaoVM entity, int nItemDetalhe, List<SubtitleAttribute> Modalidade)
        {
            var ICMSProprio = false;
            if (IsICMSProprio(detalhe))
            {
                ICMSProprio = true;
                entity.Fail(string.IsNullOrEmpty(detalhe.Imposto.ICMS.ModalidadeBC.ToString()),
                    new Error("Modalidade de determinação da base de cálculo é obrigatória para operações de ICMS próprio. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.ICMS.ModalidadeBC"));
                entity.Fail(string.IsNullOrEmpty(detalhe.Imposto.ICMS.ModalidadeBCST.ToString()),
                    new Error("Modalidade de determinação da base de cálculo do ICMS ST é obrigatória para CSOSN 201, 202, 203 e 900. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.ICMS.ModalidadeBCST"));
                entity.Fail(!string.IsNullOrEmpty(detalhe.Imposto.ICMS.ModalidadeBC.ToString()) && !Modalidade.Any(x => int.Parse(x.Value) == ((int)detalhe.Imposto.ICMS.ModalidadeBC)),
                    new Error("Modalidade de determinação da base de cálculo inválida. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.ICMS.ModalidadeBC"));
                entity.Fail(!detalhe.Imposto.ICMS.ValorBC.HasValue,
                    new Error("Base de cálculo requerida para operações de ICMS próprio. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.ICMS.ValorBC"));
                entity.Fail(!detalhe.Imposto.ICMS.AliquotaICMS.HasValue,
                    new Error("Alíquota requerida para operações de ICMS próprio. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.ICMS.ValorBC"));
                entity.Fail(!detalhe.Imposto.ICMS.ValorICMS.HasValue,
                    new Error("Valor do imposto requerido para operações de ICMS próprio. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.ICMS.ValorBC"));
            }

            return ICMSProprio;
        }

        private static bool IsICMSProprio(Detalhe detalhe)
        {
            return !string.IsNullOrEmpty(detalhe.Imposto.ICMS.ModalidadeBC.ToString()) ||
                          (detalhe.Imposto.ICMS.PercentualReducaoBC.HasValue && detalhe.Imposto.ICMS.PercentualReducaoBC > 0) ||
                          (detalhe.Imposto.ICMS.ValorBC.HasValue && detalhe.Imposto.ICMS.ValorBC > 0) ||
                          (detalhe.Imposto.ICMS.AliquotaICMS.HasValue && detalhe.Imposto.ICMS.AliquotaICMS > 0) ||
                          (detalhe.Imposto.ICMS.ValorICMS.HasValue && detalhe.Imposto.ICMS.ValorICMS > 0 ||
                          !string.IsNullOrEmpty(detalhe.Imposto.ICMS.ModalidadeBCST.ToString()));
        }

        private static void ValidarInformacoesDoCSOSNEICMSPossivelDeCredito(Detalhe detalhe, TransmissaoVM entity, int nItemDetalhe)
        {
            entity.Fail(!detalhe.Imposto.ICMS.AliquotaAplicavelCalculoCreditoSN.HasValue && detalhe.Imposto.ICMS.ValorCreditoICMS.HasValue,
                            new Error("Percentual de crédito é obrigatório para operações passíveis de crédito do ICMS. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.ICMS.AliquotaAplicavelCalculoCreditoSN"));
            entity.Fail(detalhe.Imposto.ICMS.AliquotaAplicavelCalculoCreditoSN.HasValue && !detalhe.Imposto.ICMS.ValorCreditoICMS.HasValue,
                new Error("Valor de crédito é obrigatório para operações passíveis de crédito do ICMS. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.ICMS.ValorCreditoICMS"));
        }

        private static void ValidarSubstituicaoTributaria_500(Detalhe detalhe, TransmissaoVM entity, int nItemDetalhe)
        {
            ValidarValorBaseCalculoICMS_500(detalhe, entity, nItemDetalhe);
            ValidarValorICMS_500(detalhe, entity, nItemDetalhe);
        }

        private static void ValidarValorICMS_500(Detalhe detalhe, TransmissaoVM entity, int nItemDetalhe)
        {
            entity.Fail(!detalhe.Imposto.ICMS.ValorICMSSTRetido.HasValue,
                            new Error("Valor do ICMS substituído é obrigatório para CSOSN 500. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.ICMS.ValorICMSSTRetido"));
        }

        private static void ValidarValorBaseCalculoICMS_500(Detalhe detalhe, TransmissaoVM entity, int nItemDetalhe)
        {
            entity.Fail(!detalhe.Imposto.ICMS.ValorBCSTRetido.HasValue,
                                                new Error("Valor da base de cálculo do ICMS substituído é obrigatório para CSOSN 500. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.ICMS.ValorBCSTRetido"));
        }

        private static void ValidarSubstituicaoTributaria_203(Detalhe detalhe, TransmissaoVM entity, int nItemDetalhe, ItemTransmissaoVM item, List<SubtitleAttribute> ModalidadeST)
        {
            ValidarModalidadeDeterminacao(detalhe, entity, nItemDetalhe);
            ValidarModalidadeDeterminacaoInvalida(detalhe, entity, nItemDetalhe, ModalidadeST);
            ValidarPercetualMVADoICMS(detalhe, entity, nItemDetalhe);
            ValidarValorBaseCalculoICMSST(detalhe, entity, nItemDetalhe);
            ValidarAliquotaICMSSTObrigatoria_203(detalhe, entity, nItemDetalhe);
            ValidarValorICMSSTParaCSOSN_203(detalhe, entity, nItemDetalhe);
            VerificaVersaoEValidaSubstituicaoTributaria_203(detalhe, entity, nItemDetalhe, item);
        }

        private static void VerificaVersaoEValidaSubstituicaoTributaria_203(Detalhe detalhe, TransmissaoVM entity, int nItemDetalhe, ItemTransmissaoVM item)
        {
            if (item.Versao == "4.00")
            {
                entity.Fail(!detalhe.Imposto.ICMS.BaseFCPST.HasValue,
                                new Error("Valor da Base de Cálculo do FCP retido por Substituição Tributária é obrigatório para CSOSN 201, 202, 203 e 900. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.ICMS.vBCFCPST"));
                entity.Fail(!detalhe.Imposto.ICMS.AliquotaFCPST.HasValue,
                                new Error("Percentual do FCP retido por Substituição Tributária é obrigatório para CSOSN 201, 202, 203 e 900. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.ICMS.pFCPST"));
                entity.Fail(!detalhe.Imposto.ICMS.ValorFCPST.HasValue,
                                new Error("Valor do FCP retido por Substituição Tributária é obrigatório para CSOSN 201, 202, 203 e 900. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.ICMS.vFCPST"));
            }
        }

        private static void ValidarValorICMSSTParaCSOSN_203(Detalhe detalhe, TransmissaoVM entity, int nItemDetalhe)
        {
            entity.Fail(!detalhe.Imposto.ICMS.ValorICMSST.HasValue,
                            new Error("Valor do ICMS ST é obrigatório para CSOSN 201, 202, 203 e 900. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.ICMS.ValorICMSST"));
        }

        private static void ValidarAliquotaICMSSTObrigatoria_203(Detalhe detalhe, TransmissaoVM entity, int nItemDetalhe)
        {
            entity.Fail(!detalhe.Imposto.ICMS.AliquotaICMSST.HasValue,
                            new Error("Alíquota do ICMS ST é obrigatória para CSOSN 201, 202 e 203. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.ICMS.AliquotaICMSST"));
        }

        private static void ValidarValorBaseCalculoICMSST(Detalhe detalhe, TransmissaoVM entity, int nItemDetalhe)
        {
            entity.Fail(!detalhe.Imposto.ICMS.ValorBCST.HasValue,
                            new Error("Valor da base de cálculo do ICMS ST é obrigatório para CSOSN 201, 202 e 203. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.ICMS.ValorBCST"));
        }

        private static void ValidarPercetualMVADoICMS(Detalhe detalhe, TransmissaoVM entity, int nItemDetalhe)
        {
            entity.Fail(!detalhe.Imposto.ICMS.PercentualMargemValorAdicionadoST.HasValue,
                            new Error("Percentual da MVA do ICMS ST é obrigatório para CSOSN 201, 202 e 203. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.ICMS.PercentualMargemValorAdicionadoST"));
        }

        private static void ValidarModalidadeDeterminacaoInvalida(Detalhe detalhe, TransmissaoVM entity, int nItemDetalhe, List<SubtitleAttribute> ModalidadeST)
        {
            entity.Fail(!string.IsNullOrEmpty(detalhe.Imposto.ICMS.ModalidadeBCST.ToString()) && !ModalidadeST.Any(x => int.Parse(x.Value) == ((int)detalhe.Imposto.ICMS.ModalidadeBCST)),
                            new Error("Modalidade de determinação da base de cálculo inválida. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.ICMS.ModalidadeBCST"));
        }

        private static void ValidarModalidadeDeterminacao(Detalhe detalhe, TransmissaoVM entity, int nItemDetalhe)
        {
            entity.Fail(string.IsNullOrEmpty(detalhe.Imposto.ICMS.ModalidadeBCST.ToString()),
                            new Error("Modalidade de determinação da base de cálculo do ICMS ST é obrigatória para CSOSN 201, 202, 203 e 900. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.ICMS.ModalidadeBCST"));
        }

        private static void ValidarSubstituicaoTributaria_202(Detalhe detalhe, TransmissaoVM entity, int nItemDetalhe, ItemTransmissaoVM item, List<SubtitleAttribute> ModalidadeST)
        {
            ValidarModalidadeDeterminacaoBaseDeCalculo(detalhe, entity, nItemDetalhe);
            ValidarModalidadeDeterminacaoBaseCalculoInvalida(detalhe, entity, nItemDetalhe, ModalidadeST);
            ValidarPercentualMVAICMS(detalhe, entity, nItemDetalhe);
            ValidarValorBaseCalculoICMS(detalhe, entity, nItemDetalhe);
            ValidarAliquotaICMSSTObrigatoria_202(detalhe, entity, nItemDetalhe);
            ValidarValorICMSSTParaCSOSN_202(detalhe, entity, nItemDetalhe);
            VerificaVersaoEValidaSubstituicaoTributaria_202(detalhe, entity, nItemDetalhe, item);
        }

        private static void VerificaVersaoEValidaSubstituicaoTributaria_202(Detalhe detalhe, TransmissaoVM entity, int nItemDetalhe, ItemTransmissaoVM item)
        {
            if (item.Versao == "4.00")
            {
                entity.Fail(!detalhe.Imposto.ICMS.BaseFCPST.HasValue,
                                new Error("Valor da Base de Cálculo do FCP retido por Substituição Tributária é obrigatório para CSOSN 201, 202, 203 e 900. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.ICMS.vBCFCPST"));
                entity.Fail(!detalhe.Imposto.ICMS.AliquotaFCPST.HasValue,
                                new Error("Percentual do FCP retido por Substituição Tributária é obrigatório para CSOSN 201, 202, 203 e 900. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.ICMS.pFCPST"));
                entity.Fail(!detalhe.Imposto.ICMS.ValorFCPST.HasValue,
                                new Error("Valor do FCP retido por Substituição Tributária é obrigatório para CSOSN 201, 202, 203 e 900. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.ICMS.vFCPST"));
            }
        }

        private static void ValidarValorICMSSTParaCSOSN_202(Detalhe detalhe, TransmissaoVM entity, int nItemDetalhe)
        {
            entity.Fail(!detalhe.Imposto.ICMS.ValorICMSST.HasValue,
                            new Error("Valor do ICMS ST é obrigatório para CSOSN 201, 202, 203 e 900. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.ICMS.ValorICMSST"));
        }

        private static void ValidarAliquotaICMSSTObrigatoria_202(Detalhe detalhe, TransmissaoVM entity, int nItemDetalhe)
        {
            entity.Fail(!detalhe.Imposto.ICMS.AliquotaICMSST.HasValue,
                            new Error("Alíquota do ICMS ST é obrigatória para CSOSN 201, 202 e 203. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.ICMS.AliquotaICMSST"));
        }

        private static void ValidarValorBaseCalculoICMS(Detalhe detalhe, TransmissaoVM entity, int nItemDetalhe)
        {
            entity.Fail(!detalhe.Imposto.ICMS.ValorBCST.HasValue,
                            new Error("Valor da base de cálculo do ICMS ST é obrigatório para CSOSN 201, 202 e 203. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.ICMS.ValorBCST"));
        }

        private static void ValidarPercentualMVAICMS(Detalhe detalhe, TransmissaoVM entity, int nItemDetalhe)
        {
            entity.Fail(!detalhe.Imposto.ICMS.PercentualMargemValorAdicionadoST.HasValue,
                            new Error("Percentual da MVA do ICMS ST é obrigatório para CSOSN 201, 202 e 203. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.ICMS.PercentualMargemValorAdicionadoST"));
        }

        private static void ValidarModalidadeDeterminacaoBaseCalculoInvalida(Detalhe detalhe, TransmissaoVM entity, int nItemDetalhe, List<SubtitleAttribute> ModalidadeST)
        {
            entity.Fail(!string.IsNullOrEmpty(detalhe.Imposto.ICMS.ModalidadeBCST.ToString()) && !ModalidadeST.Any(x => int.Parse(x.Value) == ((int)detalhe.Imposto.ICMS.ModalidadeBCST)),
                            new Error("Modalidade de determinação da base de cálculo inválida. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.ICMS.ModalidadeBCST"));
        }

        private static void ValidarModalidadeDeterminacaoBaseDeCalculo(Detalhe detalhe, TransmissaoVM entity, int nItemDetalhe)
        {
            entity.Fail(string.IsNullOrEmpty(detalhe.Imposto.ICMS.ModalidadeBCST.ToString()),
                            new Error("Modalidade de determinação da base de cálculo do ICMS ST é obrigatória para CSOSN 201, 202, 203 e 900. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.ICMS.ModalidadeBCST"));
        }

        private static void ValidarCasoSubstituicaoTributaria_201(Detalhe detalhe, TransmissaoVM entity, int nItemDetalhe, ItemTransmissaoVM item, List<SubtitleAttribute> ModalidadeST)
        {
            ValidarModalidadeDeterminacaoBaseCalculoObrigatoria(detalhe, entity, nItemDetalhe);
            ValidarModalidadeDeterminacaoBaseCalculo(detalhe, entity, nItemDetalhe, ModalidadeST);
            ValidarAliquotaAplicavelCalculoCredito(detalhe, entity, nItemDetalhe);
            ValidarValorCreditoICMSCSOSN(detalhe, entity, nItemDetalhe);
            ValidarPercentualMVADoICMS(detalhe, entity, nItemDetalhe);
            ValidarValorBaseDeCalculo(detalhe, entity, nItemDetalhe);
            ValidarAliquotaICMSST(detalhe, entity, nItemDetalhe);
            ValidarValorICMS(detalhe, entity, nItemDetalhe);
            VerificaVersaoEValidaICMS(detalhe, entity, nItemDetalhe, item);
        }

        private static void VerificaVersaoEValidaICMS(Detalhe detalhe, TransmissaoVM entity, int nItemDetalhe, ItemTransmissaoVM item)
        {
            if (item.Versao == "4.00")
            {
                entity.Fail(!detalhe.Imposto.ICMS.BaseFCPST.HasValue,
                                new Error("Valor da Base de Cálculo do FCP retido por Substituição Tributária é obrigatório para CSOSN 201, 202, 203 e 900. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.ICMS.vBCFCPST"));
                entity.Fail(!detalhe.Imposto.ICMS.AliquotaFCPST.HasValue,
                                new Error("Percentual do FCP retido por Substituição Tributária é obrigatório para CSOSN 201, 202, 203 e 900. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.ICMS.pFCPST"));
                entity.Fail(!detalhe.Imposto.ICMS.ValorFCPST.HasValue,
                                new Error("Valor do FCP retido por Substituição Tributária é obrigatório para CSOSN 201, 202, 203 e 900. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.ICMS.vFCPST"));
            }
        }

        private static void ValidarValorICMS(Detalhe detalhe, TransmissaoVM entity, int nItemDetalhe)
        {
            entity.Fail(!detalhe.Imposto.ICMS.ValorICMSST.HasValue,
                                    new Error("Valor do ICMS ST é obrigatório para CSOSN 201, 202, 203 e 900. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.ICMS.ValorICMSST"));
        }

        private static void ValidarAliquotaICMSST(Detalhe detalhe, TransmissaoVM entity, int nItemDetalhe)
        {
            entity.Fail(!detalhe.Imposto.ICMS.AliquotaICMSST.HasValue,
                                    new Error("Alíquota do ICMS ST é obrigatória para CSOSN 201, 202 e 203. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.ICMS.AliquotaICMSST"));
        }

        private static void ValidarValorBaseDeCalculo(Detalhe detalhe, TransmissaoVM entity, int nItemDetalhe)
        {
            entity.Fail(!detalhe.Imposto.ICMS.ValorBCST.HasValue,
                                    new Error("Valor da base de cálculo do ICMS ST é obrigatório para CSOSN 201, 202 e 203. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.ICMS.ValorBCST"));
        }

        private static void ValidarPercentualMVADoICMS(Detalhe detalhe, TransmissaoVM entity, int nItemDetalhe)
        {
            entity.Fail(!detalhe.Imposto.ICMS.PercentualMargemValorAdicionadoST.HasValue,
                                    new Error("Percentual da MVA do ICMS ST é obrigatório para CSOSN 201, 202 e 203. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.ICMS.PercentualMargemValorAdicionadoST"));
        }

        private static void ValidarValorCreditoICMSCSOSN(Detalhe detalhe, TransmissaoVM entity, int nItemDetalhe)
        {
            entity.Fail(!detalhe.Imposto.ICMS.ValorCreditoICMS.HasValue,
                                    new Error("Valor crédito do ICMS é obrigatório para CSOSN 101 e 201. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.ICMS.ValorCreditoICMS"));
        }

        private static void ValidarAliquotaAplicavelCalculoCredito(Detalhe detalhe, TransmissaoVM entity, int nItemDetalhe)
        {
            entity.Fail(!detalhe.Imposto.ICMS.AliquotaAplicavelCalculoCreditoSN.HasValue,
                                    new Error("Alíquota aplicável de cálculo do crédito é obrigatória para CSOSN 101 e 201. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.ICMS.AliquotaAplicavelCalculoCreditoSN"));
        }

        private static void ValidarModalidadeDeterminacaoBaseCalculo(Detalhe detalhe, TransmissaoVM entity, int nItemDetalhe, List<SubtitleAttribute> ModalidadeST)
        {
            entity.Fail(!string.IsNullOrEmpty(detalhe.Imposto.ICMS.ModalidadeBCST.ToString()) && !ModalidadeST.Any(x => int.Parse(x.Value) == ((int)detalhe.Imposto.ICMS.ModalidadeBCST)),
                                    new Error("Modalidade de determinação da base de cálculo inválida. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.ICMS.ModalidadeBCST"));
        }

        private static void ValidarModalidadeDeterminacaoBaseCalculoObrigatoria(Detalhe detalhe, TransmissaoVM entity, int nItemDetalhe)
        {
            entity.Fail(string.IsNullOrEmpty(detalhe.Imposto.ICMS.ModalidadeBCST.ToString()),
                                    new Error("Modalidade de determinação da base de cálculo do ICMS ST é obrigatória para CSOSN 201, 202, 203 e 900. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.ICMS.ModalidadeBCST"));
        }

        private static void ValidarOrigemDaCategoria(Detalhe detalhe, TransmissaoVM entity, int nItemDetalhe)
        {
            entity.Fail(detalhe.Imposto.ICMS.OrigemMercadoria < 0 || (int)detalhe.Imposto.ICMS.OrigemMercadoria > 8,
                                new Error("Origem da mercadoria inválida. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.ICMS.OrigemMercadoria"));
        }


        private static void ValidarLP(Detalhe detalhe, TransmissaoVM entity, int nItemDetalhe, ItemTransmissaoVM item, List<SubtitleAttribute> Modalidade)
        {            

            var ICMSProprio = ValidarInformacoesCSOSNeICMSProprio(detalhe, entity, nItemDetalhe, Modalidade);
                    
        }                   
              

    }
}
