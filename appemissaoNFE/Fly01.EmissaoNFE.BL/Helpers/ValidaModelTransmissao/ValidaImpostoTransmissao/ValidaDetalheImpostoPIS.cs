using Fly01.Core.Helpers;
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
    public static class ValidaDetalheImpostoPIS
    {
        public static void ExecutaValidaImpostoPIS(Detalhe detalhe, TransmissaoVM entity, int nItemDetalhe, ItemTransmissaoVM item)
        {
            if (detalhe.Imposto.PIS == null)
                entity.Fail(true, new Error("Os dados de PIS são obrigatórios. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.PIS"));
            else
            {
                ValidarCodigoCST(detalhe, entity, nItemDetalhe);
                ValidarPIS_IsNotOnlyCST(detalhe, entity, nItemDetalhe);
                ValidarPorSubstituicaoTributaria(detalhe, entity, nItemDetalhe);
            }
        }

        private static void ValidarPIS_IsNotOnlyCST(Detalhe detalhe, TransmissaoVM entity, int nItemDetalhe)
        {
            var OnlyCST = "04||05||06||07||08||09";

            if (!OnlyCST.Contains(((int)detalhe.Imposto.PIS.CodigoSituacaoTributaria).ToString()))
            {
                entity.Fail(string.IsNullOrEmpty(detalhe.Imposto.PIS.ValorBCDoPIS.ToString()),
                    new Error("A base de cálculo do PIS é obrigatória, para o CST informado Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.PIS.ValorBCDoPIS"));
                entity.Fail(string.IsNullOrEmpty(detalhe.Imposto.PIS.PercentualPIS.ToString()),
                    new Error("A alíquota do PIS é obrigatória, para o CST informado Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.PIS.PercentualPIS"));
                entity.Fail(string.IsNullOrEmpty(detalhe.Imposto.PIS.ValorPIS.ToString()),
                    new Error("O valor do PIS é obrigatório, para o CST informado Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.PIS.ValorPIS"));

                ValidarLengthBaseCalculoPIS_IsNotOnlyCST(detalhe, entity, nItemDetalhe);
                ValidarLengthAliquotaPisOnlyCST_IsNotOnlyCST(detalhe, entity, nItemDetalhe);
                ValidarLengthValorDoPISOnlyCST_IsNotOnlyCST(detalhe, entity, nItemDetalhe);
            }
        }

        private static void ValidarPorSubstituicaoTributaria(Detalhe detalhe, TransmissaoVM entity, int nItemDetalhe)
        {
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

                    ValidarLengthBasePisPorSubstituicaoTributaria(detalhe, entity, nItemDetalhe);
                    ValidarLengthAliquotaPorSubstituicaoTributaria(detalhe, entity, nItemDetalhe);
                    ValidarLengthValorPisPorSubstituicaoTributaria(detalhe, entity, nItemDetalhe);
                }
            }
        }

        private static void ValidarLengthValorPisPorSubstituicaoTributaria(Detalhe detalhe, TransmissaoVM entity, int nItemDetalhe)
        {
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

        private static void ValidarLengthAliquotaPorSubstituicaoTributaria(Detalhe detalhe, TransmissaoVM entity, int nItemDetalhe)
        {
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
        }

        private static void ValidarLengthBasePisPorSubstituicaoTributaria(Detalhe detalhe, TransmissaoVM entity, int nItemDetalhe)
        {
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
        }

        private static void ValidarLengthBaseCalculoPIS_IsNotOnlyCST(Detalhe detalhe, TransmissaoVM entity, int nItemDetalhe)
        {
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
        }

        private static void ValidarLengthValorDoPISOnlyCST_IsNotOnlyCST(Detalhe detalhe, TransmissaoVM entity, int nItemDetalhe)
        {
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

        private static void ValidarLengthAliquotaPisOnlyCST_IsNotOnlyCST(Detalhe detalhe, TransmissaoVM entity, int nItemDetalhe)
        {
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
        }

        private static void ValidarCodigoCST(Detalhe detalhe, TransmissaoVM entity, int nItemDetalhe)
        {
            entity.Fail(string.IsNullOrEmpty(detalhe.Imposto.PIS.CodigoSituacaoTributaria.ToString()),
                new Error("O CST do PIS é obrigatório. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.PIS.CodigoSituacaoTributaria"));
            var CSTPIS = EnumHelper.GetDataEnumValues(typeof(CSTPISCOFINS));
            entity.Fail(!string.IsNullOrEmpty(detalhe.Imposto.PIS.CodigoSituacaoTributaria.ToString()) && !CSTPIS.Any(x => int.Parse(x.Value) == ((int)detalhe.Imposto.PIS.CodigoSituacaoTributaria)),
                new Error("Código CST inválido. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto.PIS.CodigoSituacaoTributaria"));
        }
    }
}
