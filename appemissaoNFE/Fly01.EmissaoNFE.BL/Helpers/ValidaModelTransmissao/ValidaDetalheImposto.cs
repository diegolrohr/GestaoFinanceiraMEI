using Fly01.Core.Helpers;
using Fly01.Core.Notifications;
using Fly01.EmissaoNFE.BL.Helpers.ValidaModelTransmissao.ValidaImpostoTransmissao;
using Fly01.EmissaoNFE.Domain.Entities.NFe;
using Fly01.EmissaoNFE.Domain.Enums;
using Fly01.EmissaoNFE.Domain.ViewModel;
using System;
using System.Linq;

namespace Fly01.EmissaoNFE.BL.Helpers.ValidaModelTransmissao
{
    public static class ValidaDetalheImposto
    {
        public static void ExecutarValidaDetalheImposto(Detalhe detalhe, EntitiesBLToValidate entitiesBLToValidate, TransmissaoVM entity, int nItemDetalhe, ItemTransmissaoVM item)
        {
            #region Validações da classe Detalhe.Imposto

            if (detalhe.Imposto == null)
                entity.Fail(true, new Error("Os dados de imposto são obrigatórios. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto"));
            else
            {
                var totalAprox = Math.Round((detalhe.Imposto.COFINS != null ? detalhe.Imposto.COFINS.ValorCOFINS : 0) +
                                 (detalhe.Imposto.ICMS.ValorICMS ?? 0) +
                                 (detalhe.Imposto.ICMS.ValorICMSST ?? 0) +
                                 (detalhe.Imposto.ICMS.ValorFCPST ?? 0) +
                                 (detalhe.Imposto.II != null ? detalhe.Imposto.II.ValorII : 0) +
                                 (detalhe.Imposto.IPI != null ? detalhe.Imposto.IPI.ValorIPI : 0) +
                                 (detalhe.Imposto.PIS != null ? detalhe.Imposto.PIS.ValorPIS : 0) +
                                 (detalhe.Imposto.PISST != null ? detalhe.Imposto.PISST.ValorPISST : 0), 2);

                entity.Fail(!totalAprox.Equals(detalhe.Imposto.TotalAprox), new Error("Total aproximado de impostos inválido. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto"));

                ValidaDetalheImpostoICMS.ExecutaValidaImpostoICMS(detalhe,  entity, nItemDetalhe, item);

                #region Validação da classe Imposto.IPI

                if (detalhe.Imposto.IPI != null)
                {
                    entity.Fail(string.IsNullOrEmpty(detalhe.Imposto.IPI.CodigoEnquadramento),
                        new Error("Código de enquadramento legal do IPI é obrigatório. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.IPI.CodigoEnquadramento"));
                    entity.Fail(string.IsNullOrEmpty(detalhe.Imposto.IPI.CodigoST.ToString()),
                        new Error("CST do IPI é obrigatório. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.IPI.CodigoST"));
                    entity.Fail(!string.IsNullOrEmpty(detalhe.Imposto.IPI.CodigoST.ToString()) && (int)detalhe.Imposto.IPI.CodigoST < 50 && (int)item.Identificador.TipoDocumentoFiscal == 1,
                        new Error("CST do IPI inválido para uma nota de saída. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.IPI.CodigoST"));
                    entity.Fail(!string.IsNullOrEmpty(detalhe.Imposto.IPI.CodigoST.ToString()) && (int)detalhe.Imposto.IPI.CodigoST >= 50 && (int)item.Identificador.TipoDocumentoFiscal == 0,
                        new Error("CST do IPI inválido para uma nota de entrada. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.IPI.CodigoST"));
                    entity.Fail(string.IsNullOrEmpty(detalhe.Imposto.IPI.ValorBaseCalculo.ToString()),
                        new Error("Base de cálculo do IPI é obrigatória. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.IPI.ValorBaseCalculo"));
                    entity.Fail(string.IsNullOrEmpty(detalhe.Imposto.IPI.PercentualIPI.ToString()),
                        new Error("Alíquota do IPI é obrigatória. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.IPI.PercentualIPI"));
                    entity.Fail(string.IsNullOrEmpty(detalhe.Imposto.IPI.ValorIPI.ToString()),
                        new Error("Valor do IPI é obrigatório. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.IPI.ValorIPI"));
                    entity.Fail(string.IsNullOrEmpty(detalhe.Imposto.IPI.QtdTotalUnidadeTributavel.ToString()),
                        new Error("Quantidade tributada do IPI é obrigatória. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.IPI.QtdTotalUnidadeTributavel"));
                    entity.Fail(string.IsNullOrEmpty(detalhe.Imposto.IPI.ValorUnidadeTributavel.ToString()),
                        new Error("Valor por unidade tributável do IPI é obrigatório. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.IPI.ValorUnidadeTributavel"));
                }

                #endregion

                #region Validações da classe Imposto.PIS

                if (detalhe.Imposto.PIS == null)
                    entity.Fail(true, new Error("Os dados de PIS são obrigatórios. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.PIS"));
                else
                {
                    entity.Fail(string.IsNullOrEmpty(detalhe.Imposto.PIS.CodigoSituacaoTributaria.ToString()),
                        new Error("O CST do PIS é obrigatório. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.PIS.CodigoSituacaoTributaria"));
                    var CSTPIS = EnumHelper.GetDataEnumValues(typeof(CSTPISCOFINS));
                    entity.Fail(!string.IsNullOrEmpty(detalhe.Imposto.PIS.CodigoSituacaoTributaria.ToString()) && !CSTPIS.Any(x => int.Parse(x.Value) == ((int)detalhe.Imposto.PIS.CodigoSituacaoTributaria)),
                        new Error("Código CST inválido. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.PIS.CodigoSituacaoTributaria"));

                    var OnlyCST = "04||05||06||07||08||09";

                    if (!OnlyCST.Contains(((int)detalhe.Imposto.PIS.CodigoSituacaoTributaria).ToString()))
                    {
                        entity.Fail(string.IsNullOrEmpty(detalhe.Imposto.PIS.ValorBCDoPIS.ToString()),
                            new Error("A base de cálculo do PIS é obrigatória. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.PIS.ValorBCDoPIS"));
                        entity.Fail(string.IsNullOrEmpty(detalhe.Imposto.PIS.PercentualPIS.ToString()),
                            new Error("A alíquota do PIS é obrigatória. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.PIS.PercentualPIS"));
                        entity.Fail(string.IsNullOrEmpty(detalhe.Imposto.PIS.ValorPIS.ToString()),
                            new Error("O valor do PIS é obrigatório. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.PIS.ValorPIS"));

                        if (!string.IsNullOrEmpty(detalhe.Imposto.PIS.ValorBCDoPIS.ToString()))
                        {
                            string[] split = { "." };
                            var numero = detalhe.Imposto.PIS.ValorBCDoPIS.ToString().Split(split, StringSplitOptions.RemoveEmptyEntries);

                            for (int x = 0; x < numero.Length; x++)
                            {
                                if (x == 0)
                                    entity.Fail(numero[x].Length < 1 || numero[x].Length > 15,
                                        new Error("O valor da base de cálculo do PIS é inválido. (Tam. 15.2) Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.PIS.ValorBCDoPIS"));
                                else
                                    entity.Fail(numero[x] != null && numero[x].Length > 2,
                                        new Error("O número de casas decimais da base de cálculo do PIS é inválido. (Tam. 15.2) Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.PIS.ValorBCDoPIS"));
                            }
                        }
                        if (!string.IsNullOrEmpty(detalhe.Imposto.PIS.PercentualPIS.ToString()))
                        {
                            string[] split = { "." };
                            var numero = detalhe.Imposto.PIS.PercentualPIS.ToString().Split(split, StringSplitOptions.RemoveEmptyEntries);

                            for (int x = 0; x < numero.Length; x++)
                            {
                                if (x == 0)
                                    entity.Fail(numero[x].Length < 1 || numero[x].Length > 5,
                                        new Error("A alíquota do PIS é inválida. (Tam. 5.2-4) Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.PIS.PercentualPIS"));
                                else
                                    entity.Fail(numero[x] != null && (numero[x].Length < 2 || numero[x].Length > 4),
                                        new Error("O número de casas decimais da alíquota do PIS é inválido. (Tam. 5.2-4) Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.PIS.PercentualPIS"));
                            }
                        }
                        if (!string.IsNullOrEmpty(detalhe.Imposto.PIS.ValorPIS.ToString()))
                        {
                            string[] split = { "." };
                            var numero = detalhe.Imposto.PIS.ValorPIS.ToString().Split(split, StringSplitOptions.RemoveEmptyEntries);

                            for (int x = 0; x < numero.Length; x++)
                            {
                                if (x == 0)
                                    entity.Fail(numero[x].Length < 1 || numero[x].Length > 15,
                                        new Error("O valor do PIS é inválido. (Tam. 15.2) Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.PIS.ValorPIS"));
                                else
                                    entity.Fail(numero[x] != null && numero[x].Length > 2,
                                        new Error("O número de casas decimais do PIS é inválido. (Tam. 15.2) Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.PIS.ValorPIS"));
                            }
                        }
                    }

                    #region Validações da classe Imposto.PISST

                    if ((int)detalhe.Imposto.PIS.CodigoSituacaoTributaria == 5 || (int)detalhe.Imposto.PIS.CodigoSituacaoTributaria == 75)
                    {
                        entity.Fail(detalhe.Imposto.PISST == null, new Error("Os dados de PIS ST são obrigatórios para o CST 05. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.PISST"));
                        if (detalhe.Imposto.PISST != null)
                        {
                            entity.Fail(string.IsNullOrEmpty(detalhe.Imposto.PISST.ValorBC.ToString()),
                                new Error("Base do PIS ST é obrigatória. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.PISST.ValorBC"));
                            entity.Fail(string.IsNullOrEmpty(detalhe.Imposto.PISST.AliquotaPercentual.ToString()),
                                new Error("Alíquota do PIS ST é obrigatória. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.PISST.AliquotaPercentual"));
                            entity.Fail(string.IsNullOrEmpty(detalhe.Imposto.PISST.ValorPISST.ToString()),
                                new Error("Valor do PIS ST é obrigatório. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.PISST.ValorPISST"));

                            if (!string.IsNullOrEmpty(detalhe.Imposto.PISST.ValorBC.ToString()))
                            {
                                string[] split = { "." };
                                var numero = detalhe.Imposto.PISST.ValorBC.ToString().Split(split, StringSplitOptions.RemoveEmptyEntries);

                                for (int x = 0; x < numero.Length; x++)
                                {
                                    if (x == 0)
                                        entity.Fail(numero[x].Length < 1 || numero[x].Length > 15,
                                            new Error("Base do PIS ST inválida. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.PISST.ValorBC"));
                                    else
                                        entity.Fail(numero[x] != null && numero[x].Length > 2,
                                            new Error("Casas decimais inválidas na base do PIS ST. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.PISST.ValorBC"));
                                }
                            }
                            if (!string.IsNullOrEmpty(detalhe.Imposto.PISST.AliquotaPercentual.ToString()))
                            {
                                string[] split = { "." };
                                var numero = detalhe.Imposto.PISST.AliquotaPercentual.ToString().Split(split, StringSplitOptions.RemoveEmptyEntries);

                                for (int x = 0; x < numero.Length; x++)
                                {
                                    if (x == 0)
                                        entity.Fail(numero[x].Length < 1 || numero[x].Length > 15,
                                            new Error("Alíquota do PIS ST inválida. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.PISST.AliquotaPercentual"));
                                    else
                                        entity.Fail(numero[x] != null && (numero[x].Length < 2 || numero[x].Length > 4),
                                            new Error("Casas decimais inválidas na alíquota do PIS ST. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.PISST.AliquotaPercentual"));
                                }
                            }
                            if (!string.IsNullOrEmpty(detalhe.Imposto.PISST.ValorPISST.ToString()))
                            {
                                string[] split = { "." };
                                var numero = detalhe.Imposto.PISST.ValorPISST.ToString().Split(split, StringSplitOptions.RemoveEmptyEntries);

                                for (int x = 0; x < numero.Length; x++)
                                {
                                    if (x == 0)
                                        entity.Fail(numero[x].Length < 1 || numero[x].Length > 15,
                                            new Error("Valor do PIS ST inválido. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.PISST.ValorPISST"));
                                    else
                                        entity.Fail(numero[x] != null && numero[x].Length > 2,
                                            new Error("Casas decimais inválidas no valor do PIS ST. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.PISST.ValorPISST"));
                                }
                            }
                        }
                    }

                    #endregion
                }

                #endregion

            }

            #endregion

        }
    }
}
