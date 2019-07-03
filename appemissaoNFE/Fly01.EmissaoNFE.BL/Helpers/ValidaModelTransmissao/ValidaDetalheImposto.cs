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
                const string CSTsICMSST = "201||202||203||900||0||00||10||20||00||70||90||51";
                var totalAprox = Math.Round((detalhe.Imposto.COFINS != null ? detalhe.Imposto.COFINS.ValorCOFINS : 0) +
                                 (detalhe.Imposto.ICMS.ValorICMS ?? 0) +
                                 (CSTsICMSST.Contains(((int)detalhe.Imposto.ICMS.CodigoSituacaoOperacao).ToString()) ? (detalhe.Imposto.ICMS.ValorICMSST ?? 0) : 0) +
                                 (detalhe.Imposto.ICMS.ValorFCPST ?? 0) +
                                 (detalhe.Imposto.ICMS.ValorFCP ?? 0) +
                                 (detalhe.Imposto.II != null ? detalhe.Imposto.II.ValorII : 0) +
                                 (detalhe.Imposto.IPI != null ? detalhe.Imposto.IPI.ValorIPI : 0) +
                                 (detalhe.Imposto.PIS != null ? detalhe.Imposto.PIS.ValorPIS : 0) +
                                 (detalhe.Imposto.PISST != null ? detalhe.Imposto.PISST.ValorPISST : 0), 2);

                entity.Fail(!totalAprox.Equals(detalhe.Imposto.TotalAprox), new Error("Total aproximado de impostos inválido. Item: " + nItemDetalhe, "Item.Detalhes[" + (nItemDetalhe) + "].Imposto"));

                ValidaDetalheImpostoICMS.ExecutaValidaImpostoICMS(detalhe,  entity, nItemDetalhe, item);
                ValidaDetalheImpostoIPI.ExecutaValidaImpostoIPI(detalhe, entity, nItemDetalhe, item);
                ValidaDetalheImpostoPIS.ExecutaValidaImpostoPIS(detalhe, entity, nItemDetalhe, item);
            }

            #endregion
        }
    }
}
