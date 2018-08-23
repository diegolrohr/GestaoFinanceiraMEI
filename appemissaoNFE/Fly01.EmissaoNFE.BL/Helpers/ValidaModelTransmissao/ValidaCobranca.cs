using Fly01.Core.Notifications;
using Fly01.EmissaoNFE.Domain.ViewModel;
using System;
using System.Linq;

namespace Fly01.EmissaoNFE.BL.Helpers.ValidaModelTransmissao
{
    public static class ValidaCobranca
    {
        public static void ExecutarValidaCobranca(ItemTransmissaoVM item, EntitiesBLToValidate entitiesBLToValidate, TransmissaoVM entity, int nItem)
        {
            #region Validação da classe Pagamento


            if (item.Cobranca != null && item.Cobranca.Fatura == null)
                entity.Fail(true, new Error("Informe os dados da fatura. Item: " + nItem, "Item.Cobranca.Fatura"));
            else if (item.Cobranca != null && item.Cobranca.Fatura != null)
                ValidarDadosFatura(item, entity, nItem);

            #endregion Validação da classe Totais
        }

        private static void ValidarDadosFatura(ItemTransmissaoVM item, TransmissaoVM entity, int nItem)
        {
            entity.Fail(string.IsNullOrEmpty(item.Cobranca.Fatura.NumeroFatura), new Error("Informe o número da fatura.", "Item.Cobranca.Fatura.NumeroFatura"));
            entity.Fail(!string.IsNullOrEmpty(item.Cobranca.Fatura.NumeroFatura) && item.Cobranca.Fatura.NumeroFatura.Length > 60, new Error("Tamanho do número da fatura deve conter até 60 caracteres.", "Item.Cobranca.Fatura.NumeroFatura"));
            entity.Fail(!item.Cobranca.Fatura.ValorOiriginario.HasValue, new Error("Informe o valor originário.", "Item.Cobranca.Fatura.ValorOiriginario"));
            entity.Fail(item.Cobranca.Fatura.ValorOiriginario.HasValue && item.Cobranca.Fatura.ValorOiriginario <= 0, new Error("Valor originário deve ser superior a zero.", "Item.Cobranca.Fatura.ValorOiriginario"));
            entity.Fail(!item.Cobranca.Fatura.ValorLiquido.HasValue, new Error("Informe o valor líquido.", "Item.Cobranca.Fatura.ValorLiquido"));
            entity.Fail(item.Cobranca.Fatura.ValorLiquido.HasValue && item.Cobranca.Fatura.ValorLiquido <= 0, new Error("Valor líquido deve ser superior a zero.", "Item.Cobranca.Fatura.ValorLiquido"));

            if (item.Cobranca.Duplicatas == null || !item.Cobranca.Duplicatas.Any())
                entity.Fail(true, new Error("Os dados da/s duplicata/s são obrigatórios.", "Item.Cobranca.Duplicatas"));
            else
            {
                ValidarDetalheDuplicata(item, entity, nItem);
                var somaDuplicatas = item.Cobranca.Duplicatas.Sum(x => x.ValorDuplicata);
                var valorLiquido = item.Cobranca.Fatura.ValorLiquido.HasValue ? item.Cobranca.Fatura.ValorLiquido.Value : 0;

                entity.Fail(Math.Round(somaDuplicatas, 2) != Math.Round(valorLiquido, 2), new Error("O somatório do valor das duplicatas deve ser igual ao valor líquido da fatura. Item[" + nItem + "].Cobranca.Duplicatas.ValorDuplicata."));
            }
        }

        private static void ValidarDetalheDuplicata(ItemTransmissaoVM item, TransmissaoVM entity, int nItem)
        {
            var nItemDuplicata = 1;
            foreach (var duplicata in item.Cobranca.Duplicatas)
            {
                entity.Fail(string.IsNullOrEmpty(duplicata.Numero), new Error("Informe o número da duplicata. Item[" + nItem + "].Cobranca.Duplicatas[" + (nItemDuplicata) + "].Numero."));
                entity.Fail(!string.IsNullOrEmpty(duplicata.Numero) && duplicata.Numero.Length > 60, new Error("Tamanho do número da duplicata deve conter até 60 caracteres.. Item[" + nItem + "].Cobranca.Duplicatas[" + (nItemDuplicata) + "].Numero."));
                entity.Fail(duplicata.ValorDuplicata <= 0, new Error("Valor da duplicata deve ser superior a zero. Item[" + nItem + "].Cobranca.Duplicatas[" + (nItemDuplicata) + "].ValorDuplicata."));
                nItemDuplicata++;
            }
        }
    }
}